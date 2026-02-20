using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;

namespace ROFit.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoachAssignmentController : Controller
    {
        private readonly ICoachAssignmentService _service;

        public CoachAssignmentController(ICoachAssignmentService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var assignment = await _service.GetByIdAsync(id);
            if (assignment == null) return NotFound();
            return Ok(assignment);
        }

        [AllowAnonymous]
        [HttpGet("coach/{coachId}")]
        public async Task<IActionResult> GetByCoach(Guid coachId)
            => Ok(await _service.GetByCoachAsync(coachId));

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
            => Ok(await _service.GetByUserAsync(userId));

        [HttpPost]
        public async Task<IActionResult> AssignUser([FromBody] CoachAssignmentDto assignment)
        {
            assignment.Id = Guid.NewGuid();
            assignment.DateCreated = DateTime.UtcNow;
            assignment.IsActive = true;

            var id = await _service.AssignUserToCoachAsync(assignment);
            return CreatedAtAction(nameof(GetById), new { id = id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CoachAssignmentDto  assignment)
        {
            assignment.Id = id;
            assignment.DateUpdated = DateTime.UtcNow;
            if (await _service.UpdateAsync(assignment))
                return NoContent();
            return NotFound();
        }

        [HttpPatch("remove/{id}")]
        public async Task<IActionResult> Remove(Guid id, [FromBody] RemoveCoachAssignmentDto dto)
        {
            if (await _service.RemoveUserFromCoachAsync(id, dto.EndDate, dto.UpdatedBy))
                return NoContent();
            return NotFound();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveAssignments()
            => Ok(await _service.GetAllActiveAsync());
    }

    public class RemoveCoachAssignmentDto
    {
        public DateTime EndDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
