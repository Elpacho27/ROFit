using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;

[ApiController]
[Route("api/[controller]")]
public class MealPlanController : ControllerBase
{
    private readonly IMealPlanService _service;

    public MealPlanController(IMealPlanService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var plan = await _service.GetByIdAsync(id);
        if (plan == null) return NotFound();
        return Ok(plan);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetMealPlansForUser(Guid userId, string role)
    {
        var plans = await _service.GetMealPlansForUserAsync(userId, role);
        return Ok(plans);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MealPlanDto dto)
    {
        var plan = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = plan.Id }, plan);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] MealPlanDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpPut("{id}/visibility")]
    public async Task<IActionResult> UpdateVisibility(Guid id, [FromBody] bool isVisible)
    {
        var success = await _service.UpdateVisibilityAsync(id, isVisible);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
