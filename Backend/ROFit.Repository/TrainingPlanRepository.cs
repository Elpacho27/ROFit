using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;

namespace ROFit.Repository
{
    public class TrainingPlanRepository : ITrainingPlanRepository
    {
        public async Task<Guid> CreateAsync(TrainingPlanDto trainingPlan)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(TrainingPlansSqlTemplates.INSERT_PLAN, connection))
                {
                    command.Parameters.AddWithValue("@Id",trainingPlan.Id);
                    command.Parameters.AddWithValue("@Name", trainingPlan.Name);
                    command.Parameters.AddWithValue("@Description",trainingPlan.Description);
                    command.Parameters.AddWithValue("@MuscleGroup",trainingPlan.MuscleGroup);
                    command.Parameters.AddWithValue("@DateCreated",DateTime.Now);
                    command.Parameters.AddWithValue("@UserId", trainingPlan.UserId);

                    await command.ExecuteScalarAsync();

                }
                return trainingPlan.Id??Guid.Empty;
            }
        }

        public async Task<TrainingPlanDto> GetByIdAsync(Guid id)
        {
            TrainingPlanDto trainingPlan = null;
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(TrainingPlansSqlTemplates.GET_PLAN_BY_ID, connection))
                {

                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            trainingPlan = MapTrainingPlan(reader);
                        }
                    }
                }
                return trainingPlan;
            }
        }

        public async Task<List<TrainingPlanDto>> GetAllTrainingPlans()
        {
            var trainingPlans = new List<TrainingPlanDto>();
            using(var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using(var command=new NpgsqlCommand(TrainingPlansSqlTemplates.GET_ALL_ACTIVE_PLANS, connection)) 
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var exercise = MapTrainingPlan(reader);
                            trainingPlans.Add(exercise);
                        }
                    }
                }
            }
            return trainingPlans;
        }
        public async Task<bool> DeleteByIdAsync(Guid id)
        {
           using(var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(TrainingPlansSqlTemplates.HARD_DELETE_PLAN, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> UpdateAsync(TrainingPlanDto trainingPlan)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(TrainingPlansSqlTemplates.UPDATE_PLAN, connection))
                {
                    command.Parameters.AddWithValue("@Id",
                        (object?)trainingPlan.Id ?? DBNull.Value); command.Parameters.AddWithValue("@Name", trainingPlan.Name);
                    command.Parameters.AddWithValue("@Description", trainingPlan.Description);
                    command.Parameters.AddWithValue("@MuscleGroup", trainingPlan.MuscleGroup);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }

        }



        private TrainingPlanDto MapTrainingPlan(NpgsqlDataReader reader)
        {
            return new TrainingPlanDto
            {
                Id = reader.GetGuid(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Description = reader.GetString(reader.GetOrdinal("description")),
                MuscleGroup=reader.GetString(reader.GetOrdinal("musclegroup")),
                CreatedAt=reader.GetDateTime(reader.GetOrdinal("createdat"))
            };
        }

        public async Task<IEnumerable<TrainingPlanDto>> GetTrainingPlansForUserAsync(Guid userId)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                var query = @"
            SELECT * FROM trainingplans
            WHERE userid = @UserId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var list = new List<TrainingPlanDto>();

                        while (await reader.ReadAsync())
                        {
                            list.Add(MapTrainingPlan(reader));
                        }

                        return list;
                    }
                }
            }
        }
    }
}
