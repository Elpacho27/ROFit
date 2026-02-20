
namespace ROFit.Model
{
    public class TrainingPlanExerciseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int DurationSeconds { get; set; }
        public int DefaultReps { get; set; }
        public int DefaultSets { get; set; }
        public string PrimaryMuscleGroup { get; set; } = null!;
        public List<string> SecondaryMuscleGroup { get; set; } = new();

        public int DayOfWeek { get; set; }
        public bool IsCompleted { get; set; }  
        public DateTime? CompletedAt { get; set; }
    }

}
