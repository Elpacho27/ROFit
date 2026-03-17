using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;

namespace ROFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingPlanExerciseController : ControllerBase
    {
        private readonly ITrainingPlanExerciseService _service;
        private readonly IExerciseService _exerciseService;

        public TrainingPlanExerciseController(ITrainingPlanExerciseService service, IExerciseService exerciseService)
        {
            _service = service;
            _exerciseService = exerciseService;
        }

        [HttpGet("user/{userId}/plan/{trainingPlanId}")]
        public async Task<IActionResult> GetUserPlanExercises(
     Guid userId,
     Guid trainingPlanId)
        {
            var planExercises =
                await _service.GetUserPlanExercisesAsync(userId, trainingPlanId);

            var result = new List<TrainingPlanExerciseDto>();

            foreach (var planExercise in planExercises)
            {
                var exercise = await _exerciseService.GetByIdAsync(planExercise.ExerciseId);
                if (exercise == null)
                    continue;

                result.Add(new TrainingPlanExerciseDto
                {
                    Id = exercise.Id,
                    Name = exercise.Name,
                    Description = exercise.Description,
                    DurationSeconds = exercise.DurationSeconds,
                    DefaultReps = exercise.DefaultReps,
                    DefaultSets = exercise.DefaultSets,
                    PrimaryMuscleGroup = exercise.PrimaryMuscleGroup,
                    SecondaryMuscleGroup = exercise.SecondaryMuscleGroup,

                    DayOfWeek = planExercise.DayOfWeek,
                    IsCompleted = planExercise.IsCompleted,  
                    CompletedAt = planExercise.CompletedAt
                });
            }

            return Ok(new
            {
                success = true,
                exercises = result
            });
        }

        [HttpGet("daily/{userId}/{dayOfWeek}")]
        public async Task<IActionResult> GetUserDailyExercises(
     Guid userId,
     int dayOfWeek)
        {
            var exercises = await _service.GetUserDailyExercises(userId, dayOfWeek);
            return Ok(new
            {
                success = true,
                exercises = exercises
            });
        }


        [HttpGet("{userId}/{trainingPlanId}/{exerciseId}/{dayOfWeek}")]
        public async Task<IActionResult> GetExercise(
            Guid userId,
            Guid trainingPlanId,
            Guid exerciseId,
            int dayOfWeek)
        {
            var exercise = await _service.GetExerciseAsync(userId, trainingPlanId, exerciseId, dayOfWeek);

            if (exercise == null)
                return NotFound(new { success = false, message = "Exercise not found." });

            return Ok(new
            {
                success = true,
                exercise
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddExercise([FromBody] TrainingPlanExercise exercise)
        {
            var result = await _service.AddExerciseToPlanAsync(exercise);

            return Ok(new
            {
                success = result,
                message = result ? "Exercise added to training plan." : "Failed to add exercise."
            });
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateExerciseStatus(
     [FromBody] UpdateExerciseStatusDto dto)
        {
            var result = await _service.UpdateExerciseStatusAsync(
                dto.UserId,
                dto.TrainingPlanId,
                dto.ExerciseId,
                dto.DayOfWeek,
                dto.IsCompleted,
                dto.CompletedAt
            );

            return Ok(new
            {
                success = result,
                message = result
                    ? "Exercise status updated."
                    : "Failed to update status."
            });
        }

        [HttpDelete("{userId}/{trainingPlanId}/{exerciseId}/{dayOfWeek}")]
        public async Task<IActionResult> DeleteExercise(
            Guid userId,
            Guid trainingPlanId,
            Guid exerciseId,
            int dayOfWeek)
        {
            var result = await _service.DeleteExerciseFromPlanAsync(
                userId, trainingPlanId, exerciseId, dayOfWeek);

            return Ok(new
            {
                success = result,
                message = result ? "Exercise deleted from training plan." : "Failed to delete exercise."
            });
        }
    }
}
