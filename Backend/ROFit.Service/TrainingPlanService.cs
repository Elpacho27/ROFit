using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;


namespace ROFit.Service
{
    public class TrainingPlanService : ITrainingPlanService
    {
        private readonly ITrainingPlanRepository _repository;
        public TrainingPlanService(ITrainingPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateAsync(TrainingPlanDto trainingPlan)
        {
            var trainingPlanId = Guid.NewGuid();
            trainingPlan.Id= trainingPlanId;
            return await _repository.CreateAsync(trainingPlan);
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            return await _repository.DeleteByIdAsync(id);
        }

        public async Task<List<TrainingPlanDto>> GetAllTrainingPlans()
        {
            return await _repository.GetAllTrainingPlans();
        }

        public async Task<TrainingPlanDto> GetByIdAsync(Guid id)
        {
        return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(TrainingPlanDto trainingPlan)
        {
            return await _repository.UpdateAsync(trainingPlan);
        }
        public async Task<IEnumerable<TrainingPlanDto>> GetTrainingPlansForUserAsync(Guid userId)
        {
            return await _repository.GetTrainingPlansForUserAsync(userId);
        }
    }
}
