using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.SqlTemplates
{
    public static class MealPlanSqlTemplates
    {
        public const string GET_MEAL_PLAN_BY_ID = @"
            SELECT ""id"", ""user_id"", ""name"", ""description"", ""created_at"", ""calories_limit"", ""is_visible""
            FROM ""meal_plan""
            WHERE ""id"" = @Id";

        public const string GET_MEAL_PLANS_FOR_USER = @"
            SELECT ""id"", ""user_id"", ""name"", ""description"", ""created_at"", ""calories_limit"", ""is_visible""
            FROM ""meal_plan""
            WHERE ""user_id"" = @UserId 
            ORDER BY ""created_at"" DESC";

        public const string GET_VISIBLE_MEALPLANS_FOR_USER = @"
            SELECT ""id"", ""user_id"", ""name"", ""description"", ""created_at"", ""calories_limit"", ""is_visible""
            FROM ""meal_plan""
            WHERE ""user_id"" = @UserId
            AND ""is_visible"" = TRUE
            ORDER BY ""created_at"" DESC;";

        public const string INSERT_MEAL_PLAN = @"
            INSERT INTO ""meal_plan"" (""user_id"", ""name"", ""description"",""calories_limit"")
            VALUES (@UserId, @Name, @Description,@CaloriesLimit)
            RETURNING ""id""";

        public const string UPDATE_MEAL_PLAN = @"
            UPDATE ""meal_plan""
            SET ""name"" = @Name,
                ""description"" = @Description
                ""calories_limit"" = @CaloriesLimit
            WHERE ""id"" = @Id";

        public const string UPDATE_MEAL_PLAN_VISIBILITY = @"
            UPDATE ""meal_plan""
            SET ""is_visible"" = @IsVisible
            WHERE ""id"" = @Id";

        public const string DELETE_MEAL_PLAN = @"
            DELETE FROM ""meal_plan""
            WHERE ""id"" = @Id";
    }
}
