using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface IMealService
    {
        Task<MealDto> GetByIdAsync(Guid id);
        Task<IEnumerable<MealDto>> GetMealsForUserAsync(Guid userId);
        Task<MealDto> CreateAsync(MealDto dto);
        Task<bool> UpdateAsync(Guid id, MealDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
