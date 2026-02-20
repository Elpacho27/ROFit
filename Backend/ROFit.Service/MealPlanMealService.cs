using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;

public class MealPlanMealService : IMealPlanMealService
{
    private readonly IMealPlanMealRepository _repository;

    public MealPlanMealService(IMealPlanMealRepository repository)
    {
        _repository = repository;
    }

    public Task<MealPlanMealDto> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public Task<IEnumerable<MealPlanMealDto>> GetMealsForMealPlanAsync(Guid mealPlanId) => _repository.GetMealsForMealPlanAsync(mealPlanId);

    public async Task<MealPlanMealDto> CreateAsync(MealPlanMealDto dto)
    {
        dto.Id = await _repository.CreateAsync(dto);
        return dto;
    }

    public Task<bool> UpdateAsync(Guid id, MealPlanMealDto dto)
    {
        dto.Id = id;
        return _repository.UpdateAsync(dto);
    }

    public async Task<bool> DeleteAsync(Guid mealPlanId, Guid mealId)
    {
        return await _repository.DeleteAsync(mealPlanId, mealId);
    }
}
