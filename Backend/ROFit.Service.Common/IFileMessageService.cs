using Microsoft.AspNetCore.Http;
using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface IFileMessageService
    {
        Task<FileMessage> SaveFileAsync(Guid chatId, Guid senderId, IFormFile file);
        Task<(byte[] FileBytes, string FileName)> GetFileForDownloadAsync(Guid fileMessageId, Guid requesterId);
        Task<FileMessage> GetByIdAsync(Guid id);
        Task<List<FileMessage>> GetByChatIdAsync(Guid chatId);

    }
}
