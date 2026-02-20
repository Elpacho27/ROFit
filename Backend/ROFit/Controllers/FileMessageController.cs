using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;
using System.Security.Claims;

namespace ROFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FileMessagesController : ControllerBase
    {
        private readonly IFileMessageService _fileMessageService;

        public FileMessagesController(IFileMessageService fileMessageService)
        {
            _fileMessageService = fileMessageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromQuery] Guid chatId, IFormFile file)
        {
            if (file == null)
                return BadRequest("File is required");

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized("Invalid user id");

            try
            {
                var dto = await _fileMessageService.SaveFileAsync(chatId, userId, file);
                return Ok(dto);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Upload failed: {ex.Message}");
            }
        }


[HttpGet("download/{fileId}")]
    public async Task<IActionResult> Download(Guid fileId)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized("Invalid user id");

        try
        {
            var (bytes, fileName) = await _fileMessageService.GetFileForDownloadAsync(fileId, userId);

            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            var contentType = ext switch
            {
                ".pdf" => "application/pdf",
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };

            return File(bytes, contentType, fileName);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _fileMessageService.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }
        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetByChat(Guid chatId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized("Invalid user id");

            try
            {
                var list = await _fileMessageService.GetByChatIdAsync(chatId);

                var dtos = list.Select(fm => new FileMessage
                {
                    Id = fm.Id,
                    ChatId = fm.ChatId,
                    SenderId = fm.SenderId,
                    FileName = fm.FileName,
                    FileType = fm.FileType,
                    FileSize = fm.FileSize,
                    CreatedAt = fm.CreatedAt,
                    FilePath= fm.FilePath
                }).ToList();

                return Ok(new { messages = dtos });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }


}
