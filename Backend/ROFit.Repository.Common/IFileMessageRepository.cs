using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface IFileMessageRepository 
    {
        Task<List<FileMessage>> GetByChatIdAsync(Guid chatId);
        Task<FileMessage> GetByIdAsync(Guid id);
        Task<List<FileMessage>> GetBySenderIdAsync(Guid senderId);
        Task<bool> AddAsync(FileMessage fileMessage);
    }
}
