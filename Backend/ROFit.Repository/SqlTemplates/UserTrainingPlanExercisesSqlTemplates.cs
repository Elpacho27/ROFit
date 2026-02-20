using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.SqlTemplates
{
    public static class UserTrainingPlanExercisesSqlTemplates
    {
        public const string GET_USER_PLAN_DAY_PROGRESS = @"
            SELECT * FROM ""usertrainingplanexercises""
            WHERE ""userid"" = @UserId
              AND ""trainingplanid"" = @TrainingPlanId
              AND ""dayofweek"" = @DayOfWeek";

        public const string UPSERT_PROGRESS = @"
            INSERT INTO ""usertrainingplanexercises""
            (""userid"", ""trainingplanid"", ""exerciseid"", ""dayofweek"",
             ""iscompleted"", ""completedat"",
             ""datecreated"", ""dateupdated"", ""createdby"", ""updatedby"")
            VALUES (@UserId, @TrainingPlanId, @ExerciseId, @DayOfWeek,
                    @IsCompleted, @CompletedAt,
                    @DateCreated, @DateUpdated, @CreatedBy, @UpdatedBy)
            ON CONFLICT (""userid"", ""trainingplanid"", ""exerciseid"", ""dayofweek"")
            DO UPDATE
                SET ""iscompleted"" = EXCLUDED.""iscompleted"",
                    ""completedat"" = EXCLUDED.""completedat"",
                    ""dateupdated"" = EXCLUDED.""dateupdated"",
                    ""updatedby"" = EXCLUDED.""updatedby""";

        public const string RESET_PROGRESS_FOR_PLAN = @"
            DELETE FROM ""usertrainingplanexercises""
            WHERE ""userid"" = @UserId AND ""trainingplanid"" = @TrainingPlanId";
    }
}
