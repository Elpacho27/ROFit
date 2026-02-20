using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;

namespace ROFit.Repository
{
    public class FoodRepository : IFoodRepository
    {
        public async Task<IEnumerable<FoodDto>> GetAllAsync()
        {
            var foods = new List<FoodDto>();

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(FoodSqlTemplates.GET_ALL_FOOD, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            foods.Add(MapFoodFromReader(reader));
                        }
                    }
                }
            }

            return foods;
        }

        public async Task<FoodDto> GetByIdAsync(int id)
        {
            FoodDto food = null;

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(FoodSqlTemplates.GET_FOOD_BY_ID, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            food = MapFoodFromReader(reader);
                        }
                    }
                }
            }

            return food;
        }

        public async Task<int> CreateAsync(FoodDto dto)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(FoodSqlTemplates.INSERT_FOOD, connection))
                {
                    command.Parameters.AddWithValue("@Name", dto.Name);
                    command.Parameters.AddWithValue("@CaloriesPer100g", dto.CaloriesPer100g);
                    command.Parameters.AddWithValue("@ProteinPer100g", dto.ProteinPer100g);
                    command.Parameters.AddWithValue("@CarbsPer100g", dto.CarbsPer100g);
                    command.Parameters.AddWithValue("@FatPer100g", dto.FatPer100g);

                    var idObj = await command.ExecuteScalarAsync();
                    return (int)idObj;
                }
            }
        }

        public async Task<bool> UpdateAsync(FoodDto dto)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(FoodSqlTemplates.UPDATE_FOOD, connection))
                {
                    command.Parameters.AddWithValue("@Id", dto.Id);
                    command.Parameters.AddWithValue("@Name", dto.Name);
                    command.Parameters.AddWithValue("@CaloriesPer100g", dto.CaloriesPer100g);
                    command.Parameters.AddWithValue("@ProteinPer100g", dto.ProteinPer100g);
                    command.Parameters.AddWithValue("@CarbsPer100g", dto.CarbsPer100g);
                    command.Parameters.AddWithValue("@FatPer100g", dto.FatPer100g);

                    var affected = await command.ExecuteNonQueryAsync();
                    return affected > 0;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(FoodSqlTemplates.DELETE_FOOD, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    var affected = await command.ExecuteNonQueryAsync();
                    return affected > 0;
                }
            }
        }

        private FoodDto MapFoodFromReader(NpgsqlDataReader reader)
        {
            return new FoodDto
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                CaloriesPer100g = reader.GetDecimal(reader.GetOrdinal("calories_per_100g")),
                ProteinPer100g = reader.GetDecimal(reader.GetOrdinal("protein_per_100g")),
                CarbsPer100g = reader.GetDecimal(reader.GetOrdinal("carbs_per_100g")),
                FatPer100g = reader.GetDecimal(reader.GetOrdinal("fat_per_100g"))
            };
        }
    }
}
