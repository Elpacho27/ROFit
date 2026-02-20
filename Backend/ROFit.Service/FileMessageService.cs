using Microsoft.AspNetCore.Http;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;
using Microsoft.AspNetCore.Hosting;

namespace ROFit.Service
{
    public class FileMessageService : IFileMessageService
    {
        private readonly IFileMessageRepository _fileMessageRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IWebHostEnvironment _env;

        public FileMessageService(
            IFileMessageRepository fileMessageRepository,
            IChatRepository chatRepository,
            IWebHostEnvironment env)
        {
            _fileMessageRepository = fileMessageRepository;
            _chatRepository = chatRepository;
            _env = env;
        }

        public async Task<FileMessage> SaveFileAsync(Guid chatId, Guid senderId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required");

            var maxSize = 10 * 1024 * 1024; 
            if (file.Length > maxSize)
                throw new InvalidOperationException("File too large (max 10MB)");

            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat == null)
                throw new InvalidOperationException("Chat not found");

            if (chat.CoachId != senderId && chat.ClientId != senderId)
                throw new UnauthorizedAccessException("User is not in this chat");

            var safeOriginalName = Path.GetFileName(file.FileName);
            var newFileName = $"{Guid.NewGuid()}_{safeOriginalName}";
            var uploadFolder = Path.Combine("wwwroot", "uploads", "chat-files");
            Directory.CreateDirectory(uploadFolder);
            var fullPath = Path.Combine(uploadFolder, newFileName);
            var serverPath = $"/uploads/chat-files/{newFileName}";

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileMessage = new FileMessage
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                SenderId = senderId,
                FileName = safeOriginalName,
                FilePath = serverPath,
                FileType = Path.GetExtension(safeOriginalName).TrimStart('.'),
                FileSize = file.Length,
                CreatedAt = DateTime.UtcNow
            };

            var success = await _fileMessageRepository.AddAsync(fileMessage);
            if (!success)
                throw new Exception("Failed to save file message");

            return new FileMessage
            {
                Id = fileMessage.Id,
                ChatId = fileMessage.ChatId,
                SenderId = fileMessage.SenderId,
                FileName = fileMessage.FileName,
                FileType = fileMessage.FileType,
                FileSize = fileMessage.FileSize,
                CreatedAt = fileMessage.CreatedAt,
                FilePath=fileMessage.FilePath
            };
        }

        public async Task<(byte[] FileBytes, string FileName)> GetFileForDownloadAsync(
         Guid fileMessageId,
         Guid requesterId)
        {
            var fm = await _fileMessageRepository.GetByIdAsync(fileMessageId);
            if (fm == null)
                throw new FileNotFoundException("File message not found");

            var chat = await _chatRepository.GetByIdAsync(fm.ChatId);
            if (chat == null)
                throw new InvalidOperationException("Chat not found");

            if (chat.CoachId != requesterId && chat.ClientId != requesterId)
                throw new UnauthorizedAccessException("User is not in this chat");

            var relativePath = fm.FilePath.TrimStart('/'); 
            var physicalPath = Path.Combine(_env.WebRootPath,
                                            relativePath.Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(physicalPath))
                throw new FileNotFoundException("File not found on server");

            var bytes = await File.ReadAllBytesAsync(physicalPath);
            return (bytes, fm.FileName);
        }

        public async Task<FileMessage> GetByIdAsync(Guid id)
        {
            var fm = await _fileMessageRepository.GetByIdAsync(id);
            if (fm == null) return null;

            return new FileMessage
            {
                Id = fm.Id,
                ChatId = fm.ChatId,
                SenderId = fm.SenderId,
                FileName = fm.FileName,
                FileType = fm.FileType,
                FileSize = fm.FileSize,
                CreatedAt = fm.CreatedAt
            };
        }
        public async Task<List<FileMessage>> GetByChatIdAsync(Guid chatId)
        {
            return await _fileMessageRepository.GetByChatIdAsync(chatId);
        }

    }
}
