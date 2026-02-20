using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;

namespace ROFit.Service
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _repository;

        public FoodService(IFoodRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<FoodDto>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<FoodDto> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task<FoodDto> CreateAsync(FoodDto dto)
        {
            var id = await _repository.CreateAsync(dto);
            dto.Id = id;
            return dto;
        }

        public Task<bool> UpdateAsync(int id, FoodDto dto)
        {
            dto.Id = id;
            return _repository.UpdateAsync(dto);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
