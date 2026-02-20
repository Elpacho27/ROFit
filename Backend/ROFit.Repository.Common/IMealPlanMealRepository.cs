using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface IMealPlanMealRepository
    {
        Task<MealPlanMealDto> GetByIdAsync(Guid id);
        Task<IEnumerable<MealPlanMealDto>> GetMealsForMealPlanAsync(Guid mealPlanId);
        Task<Guid> CreateAsync(MealPlanMealDto dto);
        Task<bool> UpdateAsync(MealPlanMealDto dto);
        Task<bool> DeleteAsync(Guid mealPlanId, Guid mealId);
    }
}
