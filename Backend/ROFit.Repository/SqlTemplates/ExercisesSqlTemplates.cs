using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.SqlTemplates
{
    public static class ExercisesSqlTemplates
    {
        public const string GET_EXERCISE_BY_ID = @"
            SELECT * FROM ""exercises""
            WHERE ""id"" = @Id";

        public const string GET_ALL_ACTIVE_EXERCISES = @"
            SELECT * FROM ""exercises""";

        public const string INSERT_EXERCISE = @"
            INSERT INTO ""exercises"" 
            (""id"", ""name"", ""description"", ""durationseconds"", 
             ""defaultreps"", ""defaultsets"", ""primarymusclegroup"", ""secondarymusclegroup"")
            VALUES (@Id, @Name, @Description, @DurationSeconds,
                    @DefaultReps, @DefaultSets, @PrimaryMuscleGroup, @SecondaryMuscleGroup)
            RETURNING ""id""";

        public const string UPDATE_EXERCISE = @"
            UPDATE ""exercises""
            SET ""name"" = @Name,
                ""description"" = @Description,
                ""durationseconds"" = @DurationSeconds,
                ""defaultreps"" = @DefaultReps,
                ""defaultsets"" = @DefaultSets,
                ""primarymusclegroup"" = @PrimaryMuscleGroup,
                ""secondarymusclegroup"" = @SecondaryMuscleGroup
            WHERE ""id"" = @Id";

        public const string HARD_DELETE_EXERCISE = @"
            DELETE FROM ""exercises""
            WHERE ""id"" = @Id";
    }
}
