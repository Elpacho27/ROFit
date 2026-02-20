using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;

namespace ROFit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingPlanController : ControllerBase
    {
        private readonly ITrainingPlanService _service;
        public TrainingPlanController(ITrainingPlanService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TrainingPlanDto trainingPlan)
        {
            var createdPlanId = await _service.CreateAsync(trainingPlan);

            return Ok(new
            {
                success = true,
                id = createdPlanId,
                message = "Training plan created successfully."
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var trainingPlan = await _service.GetByIdAsync(id);
            if (trainingPlan == null) return NotFound();
            return Ok(new
            {
                success = true,
                trainingPlan,
            });
        }

        [HttpGet("all-training-plans")]
        public async Task<IActionResult> GetAllTrainingPlans()
        {
            var trainingPlans = await _service.GetAllTrainingPlans();
            return Ok(new
            {
                succes = true,
                trainingPlans
            });

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (!result) return NotFound();
            return Ok(new
            {
                success = true,
                message = "Training plan deleted successfully."
            });


        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] TrainingPlanDto trainingPlan)
        {
            var result = await _service.UpdateAsync(trainingPlan);
            if (!result) return NotFound();
            return Ok(new
            {
                success = true,
                message = "Training plan updated successfully."
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPlansForUser(Guid userId)
        {
            var plans = await _service.GetTrainingPlansForUserAsync(userId);

            return Ok(new
            {
                success = true,
                trainingPlans = plans
            });
        }
    }
}
