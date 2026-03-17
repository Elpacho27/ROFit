using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface ITrainingPlanExerciseService
    {
        Task<IEnumerable<TrainingPlanExercise>> GetUserPlanExercisesAsync(Guid userId, Guid trainingPlanId);
        Task<TrainingPlanExercise?> GetExerciseAsync(
            Guid userId,
            Guid trainingPlanId,
            Guid exerciseId,
            int dayOfWeek);
        Task<List<TrainingPlanExercise?>?> GetUserDailyExercises(
            Guid userId,
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
