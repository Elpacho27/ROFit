using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;

namespace ROFit.Repository
{
    public class TrainingPlanExerciseRepository : ITrainingPlanExerciseRepository
    {

        public TrainingPlanExerciseRepository()
        {
        }

        public async Task<IEnumerable<TrainingPlanExercise>> GetUserPlanExercisesAsync(Guid userId, Guid trainingPlanId)
        {
            var result = new List<TrainingPlanExercise>();

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(TrainingPlanExercisesSqlTemplates.GET_PLAN_EXERCISES, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@TrainingPlanId", trainingPlanId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new TrainingPlanExercise
                            {
                                UserId = reader.GetGuid(reader.GetOrdinal("userid")),
                                TrainingPlanId = reader.GetGuid(reader.GetOrdinal("trainingplanid")),
                                ExerciseId = reader.GetGuid(reader.GetOrdinal("exerciseid")),
                                DayOfWeek = reader.GetInt16(reader.GetOrdinal("dayofweek")),
                                IsCompleted = reader.GetBoolean(reader.GetOrdinal("iscompleted")),
                                CompletedAt = reader.IsDBNull(reader.GetOrdinal("completedat"))
                                    ? null
                                    : reader.GetDateTime(reader.GetOrdinal("completedat"))
                            });
                        }
                    }
                }
            }

            return result;
        }

        public async Task<TrainingPlanExercise?> GetExerciseAsync(Guid userId, Guid trainingPlanId, Guid exerciseId, int dayOfWeek)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                var sql = @"
                    SELECT *
                    FROM ""usertrainingplanexercises""
                    WHERE ""userid"" = @UserId
                      AND ""trainingplanid"" = @TrainingPlanId
                      AND ""exerciseid"" = @ExerciseId
                      AND ""dayofweek"" = @DayOfWeek";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@TrainingPlanId", trainingPlanId);
                    command.Parameters.AddWithValue("@ExerciseId", exerciseId);
                    command.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (!await reader.ReadAsync())
                            return null;

                        return new TrainingPlanExercise
                        {
                            UserId = reader.GetGuid(reader.GetOrdinal("userid")),
                            TrainingPlanId = reader.GetGuid(reader.GetOrdinal("trainingplanid")),
                            ExerciseId = reader.GetGuid(reader.GetOrdinal("exerciseid")),
                            DayOfWeek = reader.GetInt16(reader.GetOrdinal("dayofweek")),
                            IsCompleted = reader.GetBoolean(reader.GetOrdinal("iscompleted")),
                            CompletedAt = reader.IsDBNull(reader.GetOrdinal("completedat"))
                                ? null
                                : reader.GetDateTime(reader.GetOrdinal("completedat"))
                        };
                    }
                }
            }
        }

        public async Task<bool> AddExerciseToPlanAsync(TrainingPlanExercise exercise)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(TrainingPlanExercisesSqlTemplates.ADD_EXERCISE_TO_PLAN, connection))
                {
                    command.Parameters.AddWithValue("@UserId", exercise.UserId);
                    command.Parameters.AddWithValue("@TrainingPlanId", exercise.TrainingPlanId);
                    command.Parameters.AddWithValue("@ExerciseId", exercise.ExerciseId);
                    command.Parameters.AddWithValue("@DayOfWeek", exercise.DayOfWeek);
                    command.Parameters.AddWithValue("@IsCompleted", exercise.IsCompleted);
                    command.Parameters.AddWithValue("@CompletedAt", (object?)exercise.CompletedAt ?? DBNull.Value);

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<bool> UpdateExerciseStatusAsync(Guid userId, Guid trainingPlanId, Guid exerciseId, int dayOfWeek, bool isCompleted, DateTime? completedAt)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(TrainingPlanExercisesSqlTemplates.UPDATE_PLAN_EXERCISE, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@TrainingPlanId", trainingPlanId);
                    command.Parameters.AddWithValue("@ExerciseId", exerciseId);
                    command.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);
                    command.Parameters.AddWithValue("@IsCompleted", isCompleted);
                    command.Parameters.AddWithValue("@CompletedAt", (object?)completedAt ?? DBNull.Value);

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<bool> DeleteExerciseFromPlanAsync(Guid userId, Guid trainingPlanId, Guid exerciseId, int dayOfWeek)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(TrainingPlanExercisesSqlTemplates.DELETE_PLAN_EXERCISE, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@TrainingPlanId", trainingPlanId);
                    command.Parameters.AddWithValue("@ExerciseId", exerciseId);
                    command.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<List<TrainingPlanExercise>> GetUserDailyExercises(Guid userId, int dayOfWeek)
        {
            var exercises = new List<TrainingPlanExercise>();

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                var sql = @"
            SELECT *
            FROM ""usertrainingplanexercises""
            WHERE ""userid"" = @UserId
              AND ""dayofweek"" = @DayOfWeek";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@DayOfWeek", dayOfWeek);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            exercises.Add(new TrainingPlanExercise
                            {
                                UserId = reader.GetGuid(reader.GetOrdinal("userid")),
                                TrainingPlanId = reader.GetGuid(reader.GetOrdinal("trainingplanid")),
                                ExerciseId = reader.GetGuid(reader.GetOrdinal("exerciseid")),
                                DayOfWeek = reader.GetInt16(reader.GetOrdinal("dayofweek")),
                                IsCompleted = reader.GetBoolean(reader.GetOrdinal("iscompleted")),
                                CompletedAt = reader.IsDBNull(reader.GetOrdinal("completedat"))
                                    ? null
                                    : reader.GetDateTime(reader.GetOrdinal("completedat"))
                            });
                        }
                    }
                }
            }

            return exercises;
        }
    }
}
