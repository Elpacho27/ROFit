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
                throw new InvalidOperationException("DATABASE_URL is not set.");
            }

            var uri = new Uri(databaseUrl);
            var userInfo = uri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = uri.Host,
                Port = uri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = uri.AbsolutePath.TrimStart('/'),
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