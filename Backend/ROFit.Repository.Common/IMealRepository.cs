using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface IMealRepository
    {
        Task<MealDto> GetByIdAsync(Guid id);
        Task<IEnumerable<MealDto>> GetMealsForUserAsync(Guid userId);
        Task<Guid> CreateAsync(MealDto dto);
        Task<bool> UpdateAsync(MealDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
