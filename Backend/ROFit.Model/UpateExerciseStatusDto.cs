
namespace ROFit.Model
{
    public sealed class UpdateExerciseStatusDto
    {
        public Guid UserId { get; init; }
        public Guid TrainingPlanId { get; init; }
        public Guid ExerciseId { get; init; }
        public int DayOfWeek { get; init; }
        public bool IsCompleted { get; init; }
        public DateTime? CompletedAt { get; init; }
    }
}
