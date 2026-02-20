using Microsoft.AspNetCore.Mvc;
using ROFit.Service;

namespace ROFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly FirebaseNotificationService _firebaseService;

        public NotificationController(FirebaseNotificationService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] NotificationRequest request)
        {
            await _firebaseService.SendPushNotificationAsync(request.Title, request.Body, request.Token);
            return Ok("Notification sent successfully");
        }
    }

    public class NotificationRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
