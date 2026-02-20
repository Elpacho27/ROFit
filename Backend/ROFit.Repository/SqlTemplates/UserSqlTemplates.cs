using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.SqlTemplates
{
    public static class UserSqlTemplates
    {
        public const string GET_USER_BY_EMAIL = @"
            SELECT ""id"", ""fullname"", ""email"", ""passwordhash"", ""role"", 
                   ""datecreated"", ""dateupdated"", ""createdby"", ""updatedby"", ""isactive""
            FROM ""users"" 
            WHERE ""email"" = @Email AND ""isactive"" = true";

        public const string GET_USER_BY_ID = @"
            SELECT ""id"", ""fullname"", ""email"", ""passwordhash"", ""role"", 
                   ""datecreated"", ""dateupdated"", ""createdby"", ""updatedby"", ""isactive""
            FROM ""users"" 
            WHERE ""id"" = @Id AND ""isactive"" = true";

        public const string INSERT_USER = @"
            INSERT INTO ""users"" (""id"", ""fullname"", ""email"", ""passwordhash"", ""role"", 
                                ""datecreated"", ""dateupdated"", ""createdby"", ""updatedby"", ""isactive"") 
            VALUES (@Id, @FullName, @Email, @PasswordHash, @Role, @DateCreated, @DateUpdated, 
                    @CreatedBy, @UpdatedBy, @IsActive)
            RETURNING ""id"", ""datecreated""";

        public const string UPDATE_USER = @"
            UPDATE ""users"" 
            SET ""fullname"" = @FullName, ""email"" = @Email, ""role"" = @Role,
                ""dateupdated"" = @DateUpdated, ""updatedby"" = @UpdatedBy
            WHERE ""id"" = @Id AND ""isactive"" = true";

        public const string DELETE_USER = @"
            UPDATE ""users"" 
            SET ""isactive"" = false, ""dateupdated"" = @DateUpdated, ""updatedby"" = @UpdatedBy
            WHERE ""id"" = @Id";

        public const string GET_ALL_USERS = @"
            SELECT ""id"", ""fullname"", ""email"", ""passwordhash"", ""role"", 
                   ""datecreated"", ""dateupdated"", ""createdby"", ""updatedby"", ""isactive""
            FROM ""users"" 
            WHERE ""isactive"" = true
            ORDER BY ""datecreated"" DESC";

        public const string USER_EXISTS_BY_EMAIL = @"
            SELECT COUNT(1) FROM ""users"" WHERE ""email"" = @Email AND ""isactive"" = true";

        public const string INSERT_FCM_TOKEN = @"
            UPDATE ""users""
            SET ""fcm_tokens"" = array_append(""fcm_tokens"", @FcmToken)
            WHERE ""id"" = @Id";

        public const string GET_USER_FCM_TOKENS = @"
            SELECT ""fcm_tokens""
            FROM ""users""
            WHERE ""id"" = @Id";

    }
}
