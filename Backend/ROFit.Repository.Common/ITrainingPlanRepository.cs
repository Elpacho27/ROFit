using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface ITrainingPlanRepository
    {
        Task<Guid> CreateAsync(TrainingPlanDto trainingPlan);
        Task<TrainingPlanDto> GetByIdAsync(Guid id);
        Task<List<TrainingPlanDto>> GetAllTrainingPlans();
        Task<bool> DeleteByIdAsync(Guid id);
        Task<bool> UpdateAsync(TrainingPlanDto trainingPlan);
        Task<IEnumerable<TrainingPlanDto>> GetTrainingPlansForUserAsync(Guid userId);
    }
}
