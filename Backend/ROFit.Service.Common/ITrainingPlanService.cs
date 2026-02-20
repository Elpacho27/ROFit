using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface ITrainingPlanService
    {
        Task<Guid> CreateAsync(TrainingPlanDto trainingPlan);
        Task<TrainingPlanDto> GetByIdAsync(Guid id);
        Task<List<TrainingPlanDto>> GetAllTrainingPlans();
        Task<bool> DeleteByIdAsync(Guid id);
        Task<bool> UpdateAsync(TrainingPlanDto trainingPlan);
        Task<IEnumerable<TrainingPlanDto>> GetTrainingPlansForUserAsync(Guid userId);

    }
}
