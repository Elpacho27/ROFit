using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface IChatRepository
    {
        Task<Chat?> GetByIdAsync(Guid chatId);
        Task<Chat?> GetByCoachClientAsync(Guid coachId, Guid clientId);
        Task<Chat> AddAsync(Chat chat);
        Task<IReadOnlyList<Chat>> GetForUserAsync(Guid userId);
        
    }
}
