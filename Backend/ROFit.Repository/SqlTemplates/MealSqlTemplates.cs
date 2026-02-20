using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.SqlTemplates
{
    public static class MealSqlTemplates
    {
        public const string GET_MEAL_BY_ID = @"
            SELECT ""id"", ""user_id"", ""name"", ""date"", ""time""
            FROM ""meal""
            WHERE ""id"" = @Id";

        public const string GET_MEALS_FOR_USER = @"
            SELECT ""id"", ""user_id"", ""name"", ""date"", ""time""
            FROM ""meal""
            WHERE ""user_id"" = @UserId
            ORDER BY ""date"" DESC, ""time"" ASC";

        public const string INSERT_MEAL = @"
            INSERT INTO ""meal"" (""user_id"", ""name"", ""date"", ""time"")
            VALUES (@UserId, @Name, @Date, @Time)
            RETURNING ""id""";

        public const string UPDATE_MEAL = @"
            UPDATE ""meal""
            SET ""name"" = @Name,
                ""date"" = @Date,
                ""time"" = @Time
            WHERE ""id"" = @Id";

        public const string DELETE_MEAL = @"
            DELETE FROM ""meal""
            WHERE ""id"" = @Id";
    }
}
