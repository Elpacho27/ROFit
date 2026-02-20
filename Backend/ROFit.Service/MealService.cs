using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;
public class MealService : IMealService
{
    private readonly IMealRepository _repository;

    public MealService(IMealRepository repository)
    {
        _repository = repository;
    }

    public Task<MealDto> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public Task<IEnumerable<MealDto>> GetMealsForUserAsync(Guid userId) => _repository.GetMealsForUserAsync(userId);

    public async Task<MealDto> CreateAsync(MealDto dto)
    {
        dto.Id = await _repository.CreateAsync(dto);
        return dto;
    }

    public Task<bool> UpdateAsync(Guid id, MealDto dto)
    {
        dto.Id = id;
        return _repository.UpdateAsync(dto);
    }

    public Task<bool> DeleteAsync(Guid id) => _repository.DeleteAsync(id);
}
