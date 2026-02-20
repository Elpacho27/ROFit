using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface IMealPlanService
    {
        Task<MealPlanDto> GetByIdAsync(Guid id);
        Task<IEnumerable<MealPlanDto>> GetMealPlansForUserAsync(Guid userId, string role);
        Task<MealPlanDto> CreateAsync(MealPlanDto dto);
        Task<bool> UpdateAsync(Guid id, MealPlanDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateVisibilityAsync(Guid id, bool isVisible);
    }
}
