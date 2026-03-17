using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;

namespace ROFit.Service
{
    public class TrainingPlanExerciseService : ITrainingPlanExerciseService
    {
        private readonly ITrainingPlanExerciseRepository _repository;

        public TrainingPlanExerciseService(ITrainingPlanExerciseRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<TrainingPlanExercise>> GetUserPlanExercisesAsync(Guid userId, Guid trainingPlanId)
        {
            return _repository.GetUserPlanExercisesAsync(userId, trainingPlanId);
        }

        public Task<TrainingPlanExercise?> GetExerciseAsync(
            Guid userId,
            Guid trainingPlanId,
            Guid exerciseId,
            int dayOfWeek)
        {
            return _repository.GetExerciseAsync(userId, trainingPlanId, exerciseId, dayOfWeek);
        }

        public Task<bool> AddExerciseToPlanAsync(TrainingPlanExercise exercise)
        {
            return _repository.AddExerciseToPlanAsync(exercise);
        }

        public Task<bool> UpdateExerciseStatusAsync(
            Guid userId,
            Guid trainingPlanId,
            Guid exerciseId,
            int dayOfWeek,
            bool isCompleted,
            DateTime? completedAt)
        {
            return _repository.UpdateExerciseStatusAsync(
                userId, trainingPlanId, exerciseId, dayOfWeek, isCompleted, completedAt);
        }

        public Task<bool> DeleteExerciseFromPlanAsync(
            Guid userId,
            Guid trainingPlanId,
            Guid exerciseId,
            int dayOfWeek)
        {
            return _repository.DeleteExerciseFromPlanAsync(
                userId, trainingPlanId, exerciseId, dayOfWeek);
        }

        public Task<List<TrainingPlanExercise?>?> GetUserDailyExercises(Guid userId, int dayOfWeek)
        {
            return _repository.GetUserDailyExercises(userId,dayOfWeek);
        }
    }
}
