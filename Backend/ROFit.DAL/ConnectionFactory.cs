using Npgsql;

namespace ROFit.DAL
{
    public static class ConnectionFactory
    {
        private static readonly string ConnectionString =
    "Server = 127.0.0.1;Port=5432;Database=rofit;User Id =postgres;Password=Post27gres;";

        public static NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public static string GetConnectionString()
        {
            return ConnectionString;
        }
    }
}
