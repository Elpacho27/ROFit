using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;

[ApiController]
[Route("api/[controller]")]
public class FoodMealController : ControllerBase
{
    private readonly IFoodMealService _service;

    public FoodMealController(IFoodMealService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var foodMeal = await _service.GetByIdAsync(id);
        if (foodMeal == null) return NotFound();
        return Ok(foodMeal);
    }

    [HttpGet("meal/{mealId}")]
    public async Task<IActionResult> GetFoodMealsForMeal(Guid mealId)
    {
        var foodMeals = await _service.GetFoodMealsForMealAsync(mealId);
        return Ok(foodMeals);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FoodMealDto dto)
    {
        var foodMeal = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = foodMeal.Id }, foodMeal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] FoodMealDto dto)
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
