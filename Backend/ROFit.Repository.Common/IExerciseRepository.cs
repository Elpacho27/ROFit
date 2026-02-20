using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface IExerciseRepository
    {
        Task<Guid> CreateAsync(Exercise dto);
        Task<Exercise> GetByIdAsync(Guid id);
        Task<List<Exercise>> GetAll();
        Task<bool> UpdateExercise(Exercise exercise);
        Task<bool> DeleteById(Guid id);
    }
}
