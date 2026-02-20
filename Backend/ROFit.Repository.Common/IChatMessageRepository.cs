using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface IChatMessageRepository
    {
        Task<IReadOnlyList<ChatMessage>> GetMessagesAsync(Guid chatId, int take, int skip);
        Task<ChatMessage> AddAsync(ChatMessage message);
    }
}
