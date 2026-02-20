
namespace ROFit.Model
{
    public class TrainingPlanExercise
    {
        public Guid UserId { get; set; }
        public Guid TrainingPlanId { get; set; }
        public Guid ExerciseId { get; set; }
        public int DayOfWeek { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
