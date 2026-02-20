using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROFit.Service;
using System.Security.Claims;

namespace ROFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }
        [HttpGet("{chatId:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid chatId)
        {
            var chat = await _chatService.GetByIdAsync(chatId);
            return Ok(chat);
        }
        [HttpGet]
        public async Task<IActionResult> GetMyChats()
        {
            var userId = GetUserId();
            var chats = await _chatService.GetChatsForUserAsync(userId);
            return Ok(chats);
        }

        [HttpGet("{chatId:guid}/messages")]
        public async Task<IActionResult> GetMessages(Guid chatId, int take = 50, int skip = 0)
        {
            var userId = GetUserId();

            var messages = await _chatService.GetMessagesAsync(chatId, take, skip);
            return Ok(new { chatId, messages });
        }

        [HttpPost("coach/{coachId:guid}/client/{clientId:guid}")]
        public async Task<IActionResult> GetOrCreateChat(Guid coachId, Guid clientId)
        {
            var userId = GetUserId();
            var chat = await _chatService.GetOrCreateChatAsync(coachId, clientId);
            return Ok(chat);
        }

        private Guid GetUserId()
        {
            var userIdValue =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??  
                User.FindFirst("sub")?.Value;                        

            if (string.IsNullOrEmpty(userIdValue))
                throw new InvalidOperationException("User id claim not found.");

            return Guid.Parse(userIdValue);
        }
    }
}