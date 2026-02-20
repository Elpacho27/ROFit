using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;


[ApiController]
[Route("api/[controller]")]
public class MealController : ControllerBase
{
    private readonly IMealService _service;

    public MealController(IMealService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var meal = await _service.GetByIdAsync(id);
        if (meal == null) return NotFound();
        return Ok(meal);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetMealsForUser(Guid userId)
    {
        var meals = await _service.GetMealsForUserAsync(userId);
        return Ok(meals);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MealDto dto)
    {
        var meal = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = meal.Id }, meal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] MealDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
