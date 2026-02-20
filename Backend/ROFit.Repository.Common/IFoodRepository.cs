using ROFit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.Common
{
    public interface IFoodRepository
    {
        Task<IEnumerable<FoodDto>> GetAllAsync();
        Task<FoodDto> GetByIdAsync(int id);
        Task<int> CreateAsync(FoodDto dto);
        Task<bool> UpdateAsync(FoodDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
