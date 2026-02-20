using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface IExerciseService
    {
        Task<Exercise> CreateAsync(Exercise exercise);
        Task<Exercise> GetByIdAsync(Guid id);
        Task<List<Exercise>> GetAll();
        Task<bool> UpdateExercise(Exercise exercise);
        Task<bool> DeleteById(Guid id);
    }
}
