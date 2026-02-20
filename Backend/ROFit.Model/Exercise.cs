using ROFit.Model.Common;

namespace ROFit.Model
{
    public class Exercise : IExercise
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DurationSeconds { get; set; }
        public int DefaultReps { get; set; }
        public int DefaultSets { get; set; }
        public string PrimaryMuscleGroup { get; set; }
        public List<string> SecondaryMuscleGroup { get; set; }
    }
}
