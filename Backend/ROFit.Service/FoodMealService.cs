using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;
public class FoodMealService : IFoodMealService
{
    private readonly IFoodMealRepository _repository;

    public FoodMealService(IFoodMealRepository repository)
    {
        _repository = repository;
    }

    public Task<FoodMealDto> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public Task<IEnumerable<FoodMealWithFoodDto>> GetFoodMealsForMealAsync(Guid mealId) => _repository.GetFoodMealsForMealAsync(mealId);

    public async Task<FoodMealDto> CreateAsync(FoodMealDto dto)
    {
        dto.Id = await _repository.CreateAsync(dto);
        return dto;
    }

    public Task<bool> UpdateAsync(Guid id, FoodMealDto dto)
    {
        dto.Id = id;
        return _repository.UpdateAsync(dto);
    }

    public Task<bool> DeleteAsync(Guid id) => _repository.DeleteAsync(id);
}
