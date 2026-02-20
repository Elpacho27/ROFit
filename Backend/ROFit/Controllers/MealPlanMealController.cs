using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;


[ApiController]
[Route("api/[controller]")]
public class MealPlanMealController : ControllerBase
{
    private readonly IMealPlanMealService _service;

    public MealPlanMealController(IMealPlanMealService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var mealPlanMeal = await _service.GetByIdAsync(id);
        if (mealPlanMeal == null) return NotFound();
        return Ok(mealPlanMeal);
    }

    [HttpGet("meal_plan/{mealPlanId}")]
    public async Task<IActionResult> GetMealsForMealPlan(Guid mealPlanId)
    {
        var meals = await _service.GetMealsForMealPlanAsync(mealPlanId);
        return Ok(meals);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MealPlanMealDto dto)
    {
        var mealPlanMeal = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = mealPlanMeal.Id }, mealPlanMeal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] MealPlanMealDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] Guid mealPlanId, [FromQuery] Guid mealId)
    {
        var success = await _service.DeleteAsync(mealPlanId, mealId);
        return success ? NoContent() : NotFound();
    }

}
