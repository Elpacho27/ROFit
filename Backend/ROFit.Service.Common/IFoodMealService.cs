using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface IFoodMealService
    {
        Task<FoodMealDto> GetByIdAsync(Guid id);
        Task<IEnumerable<FoodMealWithFoodDto>> GetFoodMealsForMealAsync(Guid mealId);
        Task<FoodMealDto> CreateAsync(FoodMealDto dto);
        Task<bool> UpdateAsync(Guid id, FoodMealDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
