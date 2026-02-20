using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface IFoodService
    {
        Task<IEnumerable<FoodDto>> GetAllAsync();
        Task<FoodDto> GetByIdAsync(int id);
        Task<FoodDto> CreateAsync(FoodDto dto);
        Task<bool> UpdateAsync(int id, FoodDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
