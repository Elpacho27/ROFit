using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;

namespace ROFit.Service
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _repository;

        public ExerciseService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Exercise> CreateAsync(Exercise exercise)
        {
            var id = await _repository.CreateAsync(exercise);
            exercise.Id = id;
            return exercise;
        }

        public Task<bool> DeleteById(Guid id)
        {
            return _repository.DeleteById(id);
        }

        public  Task<List<Exercise>> GetAll()
        {
            return _repository.GetAll();
        }

        public  Task<Exercise> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<bool> UpdateExercise(Exercise exercise)
        {
           return _repository.UpdateExercise(exercise);
        }
    }
}
