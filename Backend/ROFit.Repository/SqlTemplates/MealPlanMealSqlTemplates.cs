using System;

namespace ROFit.Repository.SqlTemplates
{
    public static class MealPlanMealSqlTemplates
    {
        public const string GET_MEAL_PLAN_MEAL_BY_ID = @"
            SELECT ""id"", ""meal_plan_id"", ""meal_id"", ""meal_order""
            FROM ""meal_plan_meal""
            WHERE ""id"" = @Id";

        public const string GET_MEALS_FOR_MEAL_PLAN = @"
            SELECT mpm.""id"", mpm.""meal_plan_id"", mpm.""meal_id"", mpm.""meal_order"",
                   m.""name"", m.""date"", m.""time""
            FROM ""meal_plan_meal"" mpm
            JOIN ""meal"" m ON mpm.""meal_id"" = m.""id""
            WHERE mpm.""meal_plan_id"" = @MealPlanId
            ORDER BY mpm.""meal_order"" ASC NULLS LAST";

        public const string INSERT_MEAL_PLAN_MEAL = @"
            INSERT INTO ""meal_plan_meal"" (""meal_plan_id"", ""meal_id"", ""meal_order"")
            VALUES (@MealPlanId, @MealId, @MealOrder)
            RETURNING ""id""";

        public const string UPDATE_MEAL_PLAN_MEAL = @"
            UPDATE ""meal_plan_meal""
            SET ""meal_order"" = @MealOrder
            WHERE ""id"" = @Id";

        public const string DELETE_MEAL_PLAN_MEAL = @"
            DELETE FROM ""meal_plan_meal""
            WHERE ""meal_plan_id"" = @MealPlanId AND ""meal_id"" = @MealId";
    }
}
