using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;

namespace ROFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _service;
        public ExerciseController(IExerciseService service)
        {
            _service = service;
        }
        

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Exercise exercise)
        {
            var createdExercise = await _service.CreateAsync(exercise);
            return Ok(createdExercise);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var exercise = await _service.GetByIdAsync(id);
            if (exercise == null) return NotFound();
            return Ok(exercise);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var exercises =await _service.GetAll();
            return Ok(exercises);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Exercise exercise)
        {
            try
            {
                var isSuccess = await _service.UpdateExercise(exercise);

                if (isSuccess)
                    return Ok(new { success = true, message = "Exercise updated successfully" });
                else
                    return BadRequest(new { success = false, message = "No exercise found with given ID" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred while updating the exercise.",
                    error = ex.Message 
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var isDeleted = await _service.DeleteById(id);

                if (isDeleted)
                    return Ok(new { success = true, message = "Exercise deleted successfully" });
                else
                    return BadRequest(new { success = false, message = "No exercise found with given ID" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred while deleting the exercise.",
                    error = ex.Message
                });
            }
        }

    }
}
