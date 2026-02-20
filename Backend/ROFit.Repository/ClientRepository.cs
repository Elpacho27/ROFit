using Npgsql;
using NpgsqlTypes;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;
using System.Data;
using System.Text;

namespace ROFit.Repository
{
    public class ClientRepository : IClientRepository
    {
        public async Task<ClientDto> GetByIdAsync(Guid id)
        {
            ClientDto client = new ClientDto();

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(ClientSqlTemplates.GET_CLIENT_BY_ID, connection))
                {
                    command.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = id;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                           client= MapClientFromReader(reader);
                        }
                    }
                }
            }
            return client;
        }

        private ClientDto MapClientFromReader(NpgsqlDataReader reader)
        {
            return new ClientDto
            {
                Id = reader.GetGuid("id"),
                FullName = reader.GetString("fullname"),
                Email = reader.GetString("email"),
                Role = reader.GetString("role"),
                IsActive = reader.GetBoolean("isactive")
            };
        }
    }
}
