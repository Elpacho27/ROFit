using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.SqlTemplates
{
    public static class CoachAssignmentSqlTemplates
    {
        public const string GET_ASSIGNMENT_BY_ID = @"
            SELECT ""id"", ""userid"", ""coachid"", ""startdate"", ""enddate"", 
                   ""datecreated"", ""dateupdated"", ""createdby"", ""updatedby"", ""isactive""
            FROM ""coachassignments""
            WHERE ""id"" = @Id AND ""isactive"" = true";

        public const string GET_ASSIGNMENTS_BY_COACH = @"
            SELECT ""id"", ""userid"", ""coachid"", ""startdate"", ""enddate"", 
                   ""datecreated"", ""dateupdated"", ""createdby"", ""updatedby"", ""isactive""
            FROM ""coachassignments""
            WHERE ""coachid"" = @CoachId AND ""isactive"" = true
            ORDER BY ""datecreated"" DESC";

        public const string GET_ASSIGNMENTS_BY_USER = @"
            SELECT ""id"", ""userid"", ""coachid"", ""startdate"", ""enddate"", 
                   ""datecreated"", ""dateupdated"", ""createdby"", ""updatedby"", ""isactive""
            FROM ""coachassignments""
            WHERE ""userid"" = @UserId AND ""isactive"" = true
            ORDER BY ""datecreated"" DESC";

        public const string ASSIGN_USER_TO_COACH = @"
            INSERT INTO ""coachassignments"" (""id"", ""userid"", ""coachid"", ""startdate"", ""enddate"", 
                                              ""datecreated"", ""dateupdated"", ""createdby"", ""updatedby"", ""isactive"")
            VALUES (@Id, @UserId, @CoachId, @StartDate, @EndDate, @DateCreated, @DateUpdated, @CreatedBy, @UpdatedBy, @IsActive)
            RETURNING ""id"", ""datecreated""";

        public const string REMOVE_USER_FROM_COACH = @"
            UPDATE ""coachassignments"" 
            SET ""isactive"" = false, ""enddate"" = @EndDate, ""dateupdated"" = @DateUpdated, ""updatedby"" = @UpdatedBy
            WHERE ""id"" = @Id";

        public const string GET_ALL_ACTIVE_ASSIGNMENTS = @"
            SELECT ""id"", ""userid"", ""coachid"", ""startdate"", ""enddate"", 
                   ""datecreated"", ""dateupdated"", ""createdby"", ""updatedby"", ""isactive""
            FROM ""coachassignments""
            WHERE ""isactive"" = true
            ORDER BY ""datecreated"" DESC";

        public const string UPDATE_ASSIGNMENT = @"
    UPDATE ""coachassignments""
    SET ""userid"" = @UserId,
        ""coachid"" = @CoachId,
        ""startdate"" = @StartDate,
        ""enddate"" = @EndDate,
        ""dateupdated"" = @DateUpdated,
        ""updatedby"" = @UpdatedBy
    WHERE ""id"" = @Id AND ""isactive"" = true";

    }
}

