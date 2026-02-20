using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.SqlTemplates
{
    public static class FoodMealSqlTemplates
    {
        public const string GET_FOOD_MEAL_BY_ID = @"
            SELECT ""id"", ""meal_id"", ""food_id"", ""grams""
            FROM ""food_meal""
            WHERE ""id"" = @Id";

        public const string GET_FOOD_MEALS_FOR_MEAL = @"
            SELECT fm.""id"", fm.""meal_id"", fm.""food_id"", fm.""grams"",
                   f.""name"", f.""calories_per_100g"", f.""protein_per_100g"", f.""carbs_per_100g"", f.""fat_per_100g""
            FROM ""food_meal"" fm
            JOIN ""food"" f ON fm.""food_id"" = f.""id""
            WHERE fm.""meal_id"" = @MealId";

        public const string INSERT_FOOD_MEAL = @"
            INSERT INTO ""food_meal"" (""meal_id"", ""food_id"", ""grams"")
            VALUES (@MealId, @FoodId, @Grams)
            RETURNING ""id""";

        public const string UPDATE_FOOD_MEAL = @"
            UPDATE ""food_meal""
            SET ""grams"" = @Grams
            WHERE ""id"" = @Id";

        public const string DELETE_FOOD_MEAL = @"
            DELETE FROM ""food_meal""
            WHERE ""id"" = @Id";
    }
}
