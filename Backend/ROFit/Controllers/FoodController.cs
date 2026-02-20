using Microsoft.AspNetCore.Mvc;
using ROFit.Model;
using ROFit.Service.Common;
using System.Net.Http.Headers;

[ApiController]
[Route("api/[controller]")]
public class FoodController : ControllerBase
{
    private readonly IFoodService _service;
    private readonly HttpClient _httpClient;

    public FoodController(IFoodService service)
    {
        _service = service;
        _httpClient = new HttpClient();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var food = await _service.GetByIdAsync(id);
        if (food == null) return NotFound();
        return Ok(food);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FoodDto dto)
    {
        var food = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = food.Id }, food);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] FoodDto dto)
    {
        var success = await _service.UpdateAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
    //TEST PURPOSES ONLY
    /*[ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("analyze-food")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AnalyzeFood([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Image file is required.");

        var pythonUrl = "http://localhost:8000/predict-food";

        using var content = new MultipartFormDataContent();
        using var fileContent = new StreamContent(file.OpenReadStream());
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
        content.Add(fileContent, "file", file.FileName);

        var response = await _httpClient.PostAsync(pythonUrl, content);
        var json = await response.Content.ReadAsStringAsync();

        return Content(json, "application/json");
    }*/


}
