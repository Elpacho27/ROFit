using System;

namespace ROFit.Repository.SqlTemplates
{
    public static class TrainingPlanExercisesSqlTemplates
    {
        public const string GET_PLAN_EXERCISES = @"
            SELECT *
            FROM ""usertrainingplanexercises""
            WHERE ""trainingplanid"" = @TrainingPlanId
              AND ""userid"" = @UserId
            ORDER BY ""dayofweek""";

        public const string ADD_EXERCISE_TO_PLAN = @"
            INSERT INTO ""usertrainingplanexercises""
            (""userid"", ""trainingplanid"", ""exerciseid"", ""dayofweek"", ""iscompleted"", ""completedat"")
            VALUES (@UserId, @TrainingPlanId, @ExerciseId, @DayOfWeek, @IsCompleted, @CompletedAt)";

        public const string UPDATE_PLAN_EXERCISE = @"
            UPDATE ""usertrainingplanexercises""
            SET ""iscompleted"" = @IsCompleted,
                ""completedat"" = @CompletedAt
            WHERE ""userid"" = @UserId
              AND ""trainingplanid"" = @TrainingPlanId
              AND ""exerciseid"" = @ExerciseId
              AND ""dayofweek"" = @DayOfWeek";

        public const string DELETE_PLAN_EXERCISE = @"
            DELETE FROM ""usertrainingplanexercises""
            WHERE ""userid"" = @UserId
              AND ""trainingplanid"" = @TrainingPlanId
              AND ""exerciseid"" = @ExerciseId
              AND ""dayofweek"" = @DayOfWeek";
    }
}
