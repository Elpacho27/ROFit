using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface ITrainingPlanExerciseRepository
    {
        Task<IEnumerable<TrainingPlanExercise>> GetUserPlanExercisesAsync(Guid userId, Guid trainingPlanId);
        Task<TrainingPlanExercise?> GetExerciseAsync(
            Guid userId,
            Guid trainingPlanId,
            Guid exerciseId,
            int dayOfWeek);
        Task<bool> AddExerciseToPlanAsync(TrainingPlanExercise exercise);
        Task<bool> UpdateExerciseStatusAsync(
            Guid userId,
            Guid trainingPlanId,
            Guid exerciseId,
            int dayOfWeek,
            bool isCompleted,
            DateTime? completedAt);
        Task<bool> DeleteExerciseFromPlanAsync(
            Guid userId,
            Guid trainingPlanId,
            Guid exerciseId,
            int dayOfWeek);
    }
}
