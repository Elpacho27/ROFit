using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;


namespace ROFit.Repository
{
    public class ExerciseRepository : IExerciseRepository
    {
        public async Task<Guid> CreateAsync(Exercise exercise)
        {
            var exerciseId= Guid.NewGuid();

            using (var connection = ConnectionFactory.CreateConnection()) { 
            await connection.OpenAsync();

                using (var command = new NpgsqlCommand(ExercisesSqlTemplates.INSERT_EXERCISE, connection))
                {
                    command.Parameters.AddWithValue("@Id", exerciseId);
                    command.Parameters.AddWithValue("@Name", exercise.Name);
                    command.Parameters.AddWithValue("@Description", exercise.Description);
                    command.Parameters.AddWithValue("@DurationSeconds", exercise.DurationSeconds);
                    command.Parameters.AddWithValue("@DefaultReps", exercise.DefaultReps);
                    command.Parameters.AddWithValue("@DefaultSets", exercise.DefaultSets);
                    command.Parameters.AddWithValue("@PrimaryMuscleGroup", exercise.PrimaryMuscleGroup);
                    command.Parameters.AddWithValue("@SecondaryMuscleGroup", exercise.SecondaryMuscleGroup);

                    var idObj = await command.ExecuteScalarAsync();
                    return (Guid)idObj;
                }
            }

        }

        public async Task<bool> DeleteById(Guid id)
        {
            using(var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using(var command = new NpgsqlCommand(ExercisesSqlTemplates.HARD_DELETE_EXERCISE, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<List<Exercise>> GetAll()
        {
            var exercises = new List<Exercise>();

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(ExercisesSqlTemplates.GET_ALL_ACTIVE_EXERCISES, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var exercise = MapExerciseFromReader(reader);
                        exercises.Add(exercise);
                    }
                }
            }

            return exercises;
        }


        public async Task<Exercise> GetByIdAsync(Guid id)
        {
            Exercise? exercise = null;
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(ExercisesSqlTemplates.GET_EXERCISE_BY_ID, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            exercise = MapExerciseFromReader(reader);
                        }
                    }
                }
            }
            return exercise;

        }

        public async Task<bool> UpdateExercise(Exercise exercise)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using(var command= new NpgsqlCommand(ExercisesSqlTemplates.UPDATE_EXERCISE,connection))
                {
                    command.Parameters.AddWithValue("@Id", exercise.Id);
                    command.Parameters.AddWithValue("@Name", exercise.Name);
                    command.Parameters.AddWithValue("@Description", exercise.Description);
                    command.Parameters.AddWithValue("@DurationSeconds", exercise.DurationSeconds);
                    command.Parameters.AddWithValue("@DefaultReps", exercise.DefaultReps);
                    command.Parameters.AddWithValue("@DefaultSets", exercise.DefaultSets);
                    command.Parameters.AddWithValue("@PrimaryMuscleGroup", exercise.PrimaryMuscleGroup);
                    command.Parameters.AddWithValue("@SecondaryMuscleGroup", exercise.SecondaryMuscleGroup);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        private Exercise MapExerciseFromReader(NpgsqlDataReader reader)
        {
            return new Exercise
            {
                Id = reader.GetGuid(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Description=reader.GetString(reader.GetOrdinal("description")),
                DurationSeconds=reader.GetInt32(reader.GetOrdinal("durationseconds")),
                DefaultReps=reader.GetInt32(reader.GetOrdinal("defaultreps")),
                DefaultSets=reader.GetInt32(reader.GetOrdinal("defaultsets")),
                PrimaryMuscleGroup=reader.GetString(reader.GetOrdinal("primarymusclegroup")),
                SecondaryMuscleGroup = reader.GetFieldValue<string[]>(reader.GetOrdinal("secondarymusclegroup"))
                             .ToList()
            };
        }
    }
}
