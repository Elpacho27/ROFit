using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;

namespace ROFit.Repository
{
    public class MealRepository : IMealRepository
    {
        public async Task<MealDto> GetByIdAsync(Guid id)
        {
            MealDto meal = null;

            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealSqlTemplates.GET_MEAL_BY_ID, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                meal = MapMealFromReader(reader);
            }

            return meal;
        }

        public async Task<IEnumerable<MealDto>> GetMealsForUserAsync(Guid userId)
        {
            var meals = new List<MealDto>();

            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            string sql = 
                 MealSqlTemplates.GET_MEALS_FOR_USER;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                meals.Add(MapMealFromReader(reader));
            }

            return meals;
        }

        public async Task<Guid> CreateAsync(MealDto dto)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealSqlTemplates.INSERT_MEAL, connection);
            command.Parameters.AddWithValue("@UserId", dto.UserId?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Name", dto.Name);
            command.Parameters.AddWithValue("@Date", dto.Date);
            command.Parameters.AddWithValue("@Time", dto.Time ?? (object)DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return (Guid)result;
        }

        public async Task<bool> UpdateAsync(MealDto dto)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealSqlTemplates.UPDATE_MEAL, connection);
            command.Parameters.AddWithValue("@Id", dto.Id);
            command.Parameters.AddWithValue("@Name", dto.Name);
            command.Parameters.AddWithValue("@Date", dto.Date);
            command.Parameters.AddWithValue("@Time", dto.Time ?? (object)DBNull.Value);

            var affected = await command.ExecuteNonQueryAsync();
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(MealSqlTemplates.DELETE_MEAL, connection);
            command.Parameters.AddWithValue("@Id", id);

            var affected = await command.ExecuteNonQueryAsync();
            return affected > 0;
        }

        private MealDto MapMealFromReader(NpgsqlDataReader reader)
        {
            return new MealDto
            {
                Id = reader.GetGuid(reader.GetOrdinal("id")),
                UserId = reader.GetGuid(reader.GetOrdinal("user_id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Date = reader.GetDateTime(reader.GetOrdinal("date")),
                Time = reader.IsDBNull(reader.GetOrdinal("time"))
                    ? (TimeSpan?)null
                    : reader.GetTimeSpan(reader.GetOrdinal("time"))
            };
        }
    }
}
