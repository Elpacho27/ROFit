using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;
public class MealPlanService : IMealPlanService
{
    private readonly IMealPlanRepository _repository;

    public MealPlanService(IMealPlanRepository repository)
    {
        _repository = repository;
    }

    public Task<MealPlanDto> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public Task<IEnumerable<MealPlanDto>> GetMealPlansForUserAsync(Guid userId, string role) => _repository.GetMealPlansForUserAsync(userId, role);

    public async Task<MealPlanDto> CreateAsync(MealPlanDto dto)
    {
        dto.Id = await _repository.CreateAsync(dto);
        return dto;
    }

    public Task<bool> UpdateAsync(Guid id, MealPlanDto dto)
    {
        dto.Id = id;
        return _repository.UpdateAsync(dto);
    }

    public async Task<bool> UpdateVisibilityAsync(Guid id, bool isVisible)
    {
        return await _repository.UpdateVisibilityAsync(id, isVisible);
    }

    public Task<bool> DeleteAsync(Guid id) => _repository.DeleteAsync(id);

    
}
