
namespace ROFit.Model.Common
{
    public interface IExercise
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int DurationSeconds { get; set; }
        int DefaultReps { get; set; }
        int DefaultSets { get; set; }
        string PrimaryMuscleGroup { get; set; }
        List<string> SecondaryMuscleGroup{ get; set; }
    }
}
