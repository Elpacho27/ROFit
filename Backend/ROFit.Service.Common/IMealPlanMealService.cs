using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface IMealPlanMealService
    {
        Task<MealPlanMealDto> GetByIdAsync(Guid id);
        Task<IEnumerable<MealPlanMealDto>> GetMealsForMealPlanAsync(Guid mealPlanId);
        Task<MealPlanMealDto> CreateAsync(MealPlanMealDto dto);
        Task<bool> UpdateAsync(Guid id, MealPlanMealDto dto);
        Task<bool> DeleteAsync(Guid mealPlanId, Guid mealId);
    }
}
