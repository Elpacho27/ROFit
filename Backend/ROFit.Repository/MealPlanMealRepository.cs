using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;

namespace ROFit.Repository
{
    public class MealPlanMealRepository : IMealPlanMealRepository
    {
        public async Task<MealPlanMealDto> GetByIdAsync(Guid id)
        {
            MealPlanMealDto planMeal = null;

            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealPlanMealSqlTemplates.GET_MEAL_PLAN_MEAL_BY_ID, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                planMeal = MapMealPlanMealFromReader(reader);
            }

            return planMeal;
        }

        public async Task<IEnumerable<MealPlanMealDto>> GetMealsForMealPlanAsync(Guid mealPlanId)
        {
            var planMeals = new List<MealPlanMealDto>();

            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealPlanMealSqlTemplates.GET_MEALS_FOR_MEAL_PLAN, connection);
            command.Parameters.AddWithValue("@MealPlanId", mealPlanId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                planMeals.Add(MapMealPlanMealFromReader(reader));
            }

            return planMeals;
        }

        public async Task<Guid> CreateAsync(MealPlanMealDto dto)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealPlanMealSqlTemplates.INSERT_MEAL_PLAN_MEAL, connection);
            command.Parameters.AddWithValue("@MealPlanId", dto.MealPlanId);
            command.Parameters.AddWithValue("@MealId", dto.MealId);
            command.Parameters.AddWithValue("@MealOrder", dto.MealOrder ?? (object)DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return (Guid)result;
        }

        public async Task<bool> UpdateAsync(MealPlanMealDto dto)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealPlanMealSqlTemplates.UPDATE_MEAL_PLAN_MEAL, connection);
            command.Parameters.AddWithValue("@Id", dto.Id);
            command.Parameters.AddWithValue("@MealOrder", dto.MealOrder ?? (object)DBNull.Value);

            var affected = await command.ExecuteNonQueryAsync();
            return affected > 0;
        }

      public async Task<bool> DeleteAsync(Guid mealPlanId, Guid mealId)
{
    using var connection = ConnectionFactory.CreateConnection();
    await connection.OpenAsync();

    using var command = new NpgsqlCommand(MealPlanMealSqlTemplates.DELETE_MEAL_PLAN_MEAL, connection);

    command.Parameters.AddWithValue("@MealPlanId", mealPlanId);
    command.Parameters.AddWithValue("@MealId", mealId);

    var affected = await command.ExecuteNonQueryAsync();
    return affected > 0;
}

        private MealPlanMealDto MapMealPlanMealFromReader(NpgsqlDataReader reader)
        {
            return new MealPlanMealDto
            {
                Id = reader.GetGuid(reader.GetOrdinal("id")),
                MealPlanId = reader.GetGuid(reader.GetOrdinal("meal_plan_id")),
                MealId = reader.GetGuid(reader.GetOrdinal("meal_id")),
                MealOrder = reader.IsDBNull(reader.GetOrdinal("meal_order")) ? (short?)null : reader.GetInt16(reader.GetOrdinal("meal_order")),
                MealName = reader.IsDBNull(reader.GetOrdinal("name")) ? null : reader.GetString(reader.GetOrdinal("name")),
                MealDate = reader.IsDBNull(reader.GetOrdinal("date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("date")),
                MealTime = reader.IsDBNull(reader.GetOrdinal("time")) ? (TimeSpan?)null : reader.GetTimeSpan(reader.GetOrdinal("time"))
            };
        }
        

    }
}
