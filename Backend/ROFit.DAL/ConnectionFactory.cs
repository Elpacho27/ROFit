using Npgsql;

namespace ROFit.DAL
{
    public static class ConnectionFactory
    {
        private static readonly string ConnectionString;

        static ConnectionFactory()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            if (string.IsNullOrEmpty(databaseUrl))
            {
                throw new InvalidOperationException(
                );
            }

            var builder = new NpgsqlConnectionStringBuilder(databaseUrl)
            {
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            ConnectionString = builder.ConnectionString;
        }

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