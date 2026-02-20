using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;

namespace ROFit.Repository
{
    public class MealPlanRepository : IMealPlanRepository
    {
        public async Task<MealPlanDto> GetByIdAsync(Guid id)
        {
            MealPlanDto plan = null;

            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealPlanSqlTemplates.GET_MEAL_PLAN_BY_ID, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                plan = MapMealPlanFromReader(reader);
            }

            return plan;
        }

        public async Task<IEnumerable<MealPlanDto>> GetMealPlansForUserAsync(Guid userId, string role)
        {
            var mealPlans = new List<MealPlanDto>();

            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            string sql = role == "User"
                ? MealPlanSqlTemplates.GET_VISIBLE_MEALPLANS_FOR_USER
                : MealPlanSqlTemplates.GET_MEAL_PLANS_FOR_USER;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                mealPlans.Add(MapMealPlanFromReader(reader));
            }

            return mealPlans;
        }

        public async Task<Guid> CreateAsync(MealPlanDto dto)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealPlanSqlTemplates.INSERT_MEAL_PLAN, connection);
            command.Parameters.AddWithValue("@UserId", dto.UserId);
            command.Parameters.AddWithValue("@Name", dto.Name);
            command.Parameters.AddWithValue("@Description", dto.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CaloriesLimit", dto.CaloriesLimit);

            var result = await command.ExecuteScalarAsync();
            return (Guid)result;
        }

        public async Task<bool> UpdateAsync(MealPlanDto dto)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealPlanSqlTemplates.UPDATE_MEAL_PLAN, connection);
            command.Parameters.AddWithValue("@Id", dto.Id);
            command.Parameters.AddWithValue("@Name", dto.Name);
            command.Parameters.AddWithValue("@Description", dto.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CaloriesLimit", dto.CaloriesLimit);


            var affected = await command.ExecuteNonQueryAsync();
            return affected > 0;
        }

        public async Task<bool> UpdateVisibilityAsync(Guid id, bool isVisible)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealPlanSqlTemplates.UPDATE_MEAL_PLAN_VISIBILITY, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@IsVisible", isVisible);

            var affected = await command.ExecuteNonQueryAsync();
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealPlanSqlTemplates.DELETE_MEAL_PLAN, connection);
            command.Parameters.AddWithValue("@Id", id);

            var affected = await command.ExecuteNonQueryAsync();
            return affected > 0;
        }

        private MealPlanDto MapMealPlanFromReader(NpgsqlDataReader reader)
        {
            return new MealPlanDto
            {
                Id = reader.GetGuid(reader.GetOrdinal("id")),
                UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                CaloriesLimit = reader.GetInt16(reader.GetOrdinal("calories_limit")),
                IsVisible = reader.GetBoolean(reader.GetOrdinal("is_visible"))  
            };
        }
    }
}
