using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROFit.Repository.SqlTemplates
{
    public static class ClientSqlTemplates
    {

        public const string GET_CLIENT_BY_ID = @"
            SELECT ""id"", ""fullname"", ""email"", ""role"", 
                   ""isactive""
            FROM ""users"" 
            WHERE ""id"" = @Id AND ""isactive"" = true";


    }
}
