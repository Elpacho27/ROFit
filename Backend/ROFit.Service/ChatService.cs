using ROFit.Model;
using ROFit.Repository.Common;

namespace ROFit.Service
{
    public class ChatService
    {
        private readonly IChatRepository _chatRepo;
        private readonly IChatMessageRepository _msgRepo;

        public ChatService(IChatRepository chatRepo, IChatMessageRepository msgRepo)
        {
            _chatRepo = chatRepo;
            _msgRepo = msgRepo;
        }

        public async Task<Chat> GetOrCreateChatAsync(Guid coachId, Guid clientId)
        {
            var chat = await _chatRepo.GetByCoachClientAsync(coachId, clientId);
            if (chat != null) return chat;

            chat = new Chat
            {
                CoachId = coachId,
                ClientId = clientId,
                CreatedAt = DateTime.UtcNow
            };
            return await _chatRepo.AddAsync(chat);
        }
        public async Task<Chat?> GetByIdAsync(Guid chatId)
        {
            var chat = await _chatRepo.GetByIdAsync(chatId);
            return chat;
        }

        public Task<IReadOnlyList<Chat>> GetChatsForUserAsync(Guid userId) =>
            _chatRepo.GetForUserAsync(userId);

        public async Task<ChatMessage> AddMessageAsync(Guid chatId, Guid senderId, string content)
        {
            var msg = new ChatMessage
            {
                ChatId = chatId,
                SenderId = senderId,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };
            return await _msgRepo.AddAsync(msg);
        }

        public Task<IReadOnlyList<ChatMessage>> GetMessagesAsync(Guid chatId, int take = 50, int skip = 0) =>
            _msgRepo.GetMessagesAsync(chatId, take, skip);
    }

}
