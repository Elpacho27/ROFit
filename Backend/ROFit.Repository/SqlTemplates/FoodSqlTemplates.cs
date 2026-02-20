using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.SqlTemplates
{
    public static class FoodSqlTemplates
    {
        public const string GET_FOOD_BY_ID = @"
            SELECT ""id"", ""name"", ""calories_per_100g"", ""protein_per_100g"",
                   ""carbs_per_100g"", ""fat_per_100g""
            FROM ""food""
            WHERE ""id"" = @Id";

        public const string GET_ALL_FOOD = @"
            SELECT ""id"", ""name"", ""calories_per_100g"", ""protein_per_100g"",
                   ""carbs_per_100g"", ""fat_per_100g""
            FROM ""food""
            ORDER BY ""name"" ASC";

        public const string INSERT_FOOD = @"
            INSERT INTO ""food"" (""name"", ""calories_per_100g"", ""protein_per_100g"", 
                                  ""carbs_per_100g"", ""fat_per_100g"")
            VALUES (@Name, @CaloriesPer100g, @ProteinPer100g, @CarbsPer100g, @FatPer100g)
            RETURNING ""id""";

        public const string UPDATE_FOOD = @"
            UPDATE ""food""
            SET ""name"" = @Name,
                ""calories_per_100g"" = @CaloriesPer100g,
                ""protein_per_100g"" = @ProteinPer100g,
                ""carbs_per_100g"" = @CarbsPer100g,
                ""fat_per_100g"" = @FatPer100g
            WHERE ""id"" = @Id";

        public const string DELETE_FOOD = @"
            DELETE FROM ""food""
            WHERE ""id"" = @Id";
    }

}
