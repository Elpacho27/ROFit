using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface IFoodMealRepository
    {
        Task<FoodMealDto> GetByIdAsync(Guid id);
        Task<IEnumerable<FoodMealWithFoodDto>> GetFoodMealsForMealAsync(Guid mealId);
        Task<Guid> CreateAsync(FoodMealDto dto);
        Task<bool> UpdateAsync(FoodMealDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
