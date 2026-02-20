using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;

namespace ROFit.Repository
{
    public class FoodMealRepository : IFoodMealRepository
    {
        public async Task<FoodMealDto> GetByIdAsync(Guid id)
        {
            FoodMealDto foodMeal = null;

            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(FoodMealSqlTemplates.GET_FOOD_MEAL_BY_ID, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                foodMeal = MapFoodMealFromReader(reader);
            }

            return foodMeal;
        }

        public async Task<IEnumerable<FoodMealWithFoodDto>> GetFoodMealsForMealAsync(Guid mealId)
        {
            var list = new List<FoodMealWithFoodDto>();
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();
            using var command = new NpgsqlCommand(FoodMealSqlTemplates.GET_FOOD_MEALS_FOR_MEAL, connection);
            command.Parameters.AddWithValue("@MealId", mealId);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(MapFoodMealWithFoodFromReader(reader));
            }
            return list;
        }

        public async Task<Guid> CreateAsync(FoodMealDto dto)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(FoodMealSqlTemplates.INSERT_FOOD_MEAL, connection);
            command.Parameters.AddWithValue("@MealId", dto.MealId);
            command.Parameters.AddWithValue("@FoodId", dto.FoodId);
            command.Parameters.AddWithValue("@Grams", dto.Grams);

            var result = await command.ExecuteScalarAsync();
            return (Guid)result;
        }

        public async Task<bool> UpdateAsync(FoodMealDto dto)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(FoodMealSqlTemplates.UPDATE_FOOD_MEAL, connection);
            command.Parameters.AddWithValue("@Id", dto.Id);
            command.Parameters.AddWithValue("@Grams", dto.Grams);

            var affected = await command.ExecuteNonQueryAsync();
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = ConnectionFactory.CreateConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(FoodMealSqlTemplates.DELETE_FOOD_MEAL, connection);
            command.Parameters.AddWithValue("@Id", id);

            var affected = await command.ExecuteNonQueryAsync();
            return affected > 0;
        }

        private FoodMealDto MapFoodMealFromReader(NpgsqlDataReader reader)
        {
            return new FoodMealDto
            {
                Id = reader.GetGuid(reader.GetOrdinal("id")),
                MealId = reader.GetGuid(reader.GetOrdinal("meal_id")),
                FoodId = reader.GetInt32(reader.GetOrdinal("food_id")),
                Grams = reader.GetDecimal(reader.GetOrdinal("grams"))
            };
        }

        private FoodMealWithFoodDto MapFoodMealWithFoodFromReader(NpgsqlDataReader reader)
        {
            return new FoodMealWithFoodDto
            {
                Id = reader.GetGuid(reader.GetOrdinal("id")),
                MealId = reader.GetGuid(reader.GetOrdinal("meal_id")),
                FoodId = reader.GetInt32(reader.GetOrdinal("food_id")),
                Grams = reader.GetDecimal(reader.GetOrdinal("grams")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                CaloriesPer100g = reader.GetDecimal(reader.GetOrdinal("calories_per_100g")),
                ProteinPer100g = reader.GetDecimal(reader.GetOrdinal("protein_per_100g")),
                CarbsPer100g = reader.GetDecimal(reader.GetOrdinal("carbs_per_100g")),
                FatPer100g = reader.GetDecimal(reader.GetOrdinal("fat_per_100g")),
            };
        }

    }
}
