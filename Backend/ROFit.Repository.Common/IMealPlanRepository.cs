using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface IMealPlanRepository
    {
        Task<MealPlanDto> GetByIdAsync(Guid id);
        Task<IEnumerable<MealPlanDto>> GetMealPlansForUserAsync(Guid userId, string role);
        Task<Guid> CreateAsync(MealPlanDto dto);
        Task<bool> UpdateAsync(MealPlanDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateVisibilityAsync(Guid id, bool isVisible);
    }
}
