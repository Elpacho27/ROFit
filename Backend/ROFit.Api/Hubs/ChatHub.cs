using Microsoft.AspNetCore.SignalR;
using ROFit.common;
using ROFit.Service;
using ROFit.Service.Common;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace ROFit.Api.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        private readonly FirebaseNotificationService _firebaseNotificationService;
        private readonly IFileMessageService _fileMessageService;
        private readonly IUserService _userService;
        private static readonly ConcurrentDictionary<Guid, HashSet<Guid>> ChatPresence
    = new();
        public ChatHub(ChatService chatService, IFileMessageService fileMessageService, FirebaseNotificationService firebaseNotificationService, IUserService userService)
        {
            _chatService = chatService;
            _fileMessageService = fileMessageService;
            _firebaseNotificationService = firebaseNotificationService;
            _userService = userService;
        }
        public async Task SendFileMessage(Guid chatId, Guid fileMessageId)
        {
            var dto = await _fileMessageService.GetByIdAsync(fileMessageId);
            if (dto == null) return;

            await Clients.Group(chatId.ToString()).SendAsync(
                HubEvents.ReceiveFileMessage,dto);
        }
        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            var chats = await _chatService.GetChatsForUserAsync(userId);

            foreach (var chat in chats)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
            }

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = GetUserId();

            foreach (var entry in ChatPresence)
            {
                lock (entry.Value)
                {
                    entry.Value.Remove(userId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
        public async Task JoinChat(Guid chatId)
        {
            var userId = GetUserId();
            var chats = await _chatService.GetChatsForUserAsync(userId);

            if (!chats.Any(c => c.Id == chatId))
                throw new HubException("Not allowed to join this chat.");

            var users = ChatPresence.GetOrAdd(chatId, _ => new HashSet<Guid>());
            lock (users)
            {
                users.Add(userId);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        public async Task StartTyping(string chatId)
        {
            await Clients.OthersInGroup(chatId)
                .SendAsync(HubEvents.UserTyping, chatId, true);
        }

        public async Task StopTyping(string chatId)
        {
            await Clients.OthersInGroup(chatId)
                .SendAsync(HubEvents.UserTyping, chatId, false);
        }

        public async Task LeaveChat(Guid chatId)
        {
            var userId = GetUserId();

            if (ChatPresence.TryGetValue(chatId, out var users))
            {
                lock (users)
                {
                    users.Remove(userId);
                    if (users.Count == 0)
                        ChatPresence.TryRemove(chatId, out _);
                }
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        public async Task SendMessage(Guid chatId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;

            var senderId = GetUserId();

            var message = await _chatService.AddMessageAsync(chatId, senderId, content);

            await Clients.Group(chatId.ToString()).SendAsync(HubEvents.ReceiveMessage, new
            {
                id = message.Id,
                chatId = message.ChatId,
                senderId = message.SenderId,
                content = message.Content,
                createdAt = message.CreatedAt,
                isRead = message.IsRead
            });

            var chat = await _chatService.GetByIdAsync(chatId);
            var sender = await _userService.GetByIdAsync(senderId);
            var receiverId =
                chat.ClientId == senderId
                    ? chat.CoachId
                    : chat.ClientId;

            var receiverConnected =
                ChatPresence.TryGetValue(chatId, out var users) &&
                users.Contains(receiverId);

            if (!receiverConnected)
            {
                var tokens = await _userService.GetAllUserFcmTokens(receiverId);

                foreach (var token in tokens)
                {
                    await _firebaseNotificationService.SendPushNotificationAsync(
                        sender.FullName,
                        content,
                        token
                    );
                }
            }
        }


        private Guid GetUserId()
        {
            var userIdValue =
                Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                Context.User?.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdValue))
                throw new HubException("User id claim not found.");

            return Guid.Parse(userIdValue);
        }
    }
}
