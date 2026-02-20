using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.SqlTemplates
{
    public static class TrainingPlansSqlTemplates
    {
        public const string GET_PLAN_BY_ID = @"
            SELECT * FROM ""trainingplans""
            WHERE ""id"" = @Id";

        public const string GET_ALL_ACTIVE_PLANS = @"
            SELECT * FROM ""trainingplans""
            ORDER BY ""createdat"" DESC";

        public const string INSERT_PLAN = @"
            INSERT INTO ""trainingplans"" 
            (""id"", ""name"", ""description"", ""musclegroup"",
             ""createdat"",""userid"")
            VALUES (@Id, @Name, @Description, @MuscleGroup,
                    @DateCreated,@UserId)
            RETURNING ""id"", ""createdat""";

        public const string UPDATE_PLAN = @"
            UPDATE ""trainingplans""
            SET ""name"" = @Name,
                ""description"" = @Description,
                ""musclegroup"" = @MuscleGroup
            WHERE ""id"" = @Id";

        public const string HARD_DELETE_PLAN = @"
            DELETE FROM ""trainingplans""
            WHERE ""id"" = @Id";
    }
}
