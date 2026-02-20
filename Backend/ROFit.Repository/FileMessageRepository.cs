using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using System.Data;

namespace ROFit.Repository
{
    public class FileMessageRepository : IFileMessageRepository
    {

        public FileMessageRepository()
        {
        }
        private NpgsqlConnection CreateConnection() => ConnectionFactory.CreateConnection();

        public async Task<bool> AddAsync(FileMessage fileMessage)
        {
            const string query = @"
                INSERT INTO FileMessages (Id, ChatId, SenderId, FileName, FilePath, FileType, FileSize, CreatedAt)
                VALUES (@Id, @ChatId, @SenderId, @FileName, @FilePath, @FileType, @FileSize, @CreatedAt)";

            await using var conn = CreateConnection();

            await conn.OpenAsync();
                using (var command = new NpgsqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@Id", fileMessage.Id);
                    command.Parameters.AddWithValue("@ChatId", fileMessage.ChatId);
                    command.Parameters.AddWithValue("@SenderId", fileMessage.SenderId);
                    command.Parameters.AddWithValue("@FileName", fileMessage.FileName ?? "");
                    command.Parameters.AddWithValue("@FilePath", fileMessage.FilePath ?? "");
                    command.Parameters.AddWithValue("@FileType", fileMessage.FileType ?? "");
                    command.Parameters.AddWithValue("@FileSize", fileMessage.FileSize);
                    command.Parameters.AddWithValue("@CreatedAt", fileMessage.CreatedAt);

                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            
        }

        public async Task<FileMessage> GetByIdAsync(Guid id)
        {
            const string query = @"
                SELECT Id, ChatId, SenderId, FileName, FilePath, FileType, FileSize, CreatedAt
                FROM FileMessages
                WHERE Id = @Id";

            await using var conn = CreateConnection();

            await conn.OpenAsync();
            using (var command = new NpgsqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapFileMessage(reader);
                        }
                    }
            }
            
            return null;
        }

        public async Task<List<FileMessage>> GetByChatIdAsync(Guid chatId)
        {
            const string query = @"
                SELECT Id, ChatId, SenderId, FileName, FilePath, FileType, FileSize, CreatedAt
                FROM FileMessages
                WHERE ChatId = @ChatId
                ORDER BY CreatedAt DESC";

            var fileMessages = new List<FileMessage>();

            await using var conn = CreateConnection();

            await conn.OpenAsync();
            using (var command = new NpgsqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@ChatId", chatId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            fileMessages.Add(MapFileMessage(reader));
                        }
                    }
            }
            

            return fileMessages;
        }

        public async Task<List<FileMessage>> GetBySenderIdAsync(Guid senderId)
        {
            const string query = @"
                SELECT Id, ChatId, SenderId, FileName, FilePath, FileType, FileSize, CreatedAt
                FROM FileMessages
                WHERE SenderId = @SenderId
                ORDER BY CreatedAt DESC";

            var fileMessages = new List<FileMessage>();

            await using var conn = CreateConnection();

            await conn.OpenAsync();
            using (var command = new NpgsqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@SenderId", senderId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            fileMessages.Add(MapFileMessage(reader));
                        }
                    }
            }
            

            return fileMessages;
        }

        public async Task<bool> UpdateAsync(FileMessage fileMessage)
        {
            const string query = @"
                UPDATE FileMessages
                SET ChatId = @ChatId, SenderId = @SenderId, FileName = @FileName, 
                    FilePath = @FilePath, FileType = @FileType, FileSize = @FileSize
                WHERE Id = @Id";

            await using var conn = CreateConnection();

            await conn.OpenAsync();
            using (var command = new NpgsqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Id", fileMessage.Id);
                    command.Parameters.AddWithValue("@ChatId", fileMessage.ChatId);
                    command.Parameters.AddWithValue("@SenderId", fileMessage.SenderId);
                    command.Parameters.AddWithValue("@FileName", fileMessage.FileName ?? "");
                    command.Parameters.AddWithValue("@FilePath", fileMessage.FilePath ?? "");
                    command.Parameters.AddWithValue("@FileType", fileMessage.FileType ?? "");
                    command.Parameters.AddWithValue("@FileSize", fileMessage.FileSize);

                    var result = await command.ExecuteNonQueryAsync();
                    return result > 0;
            }
        }
        

        public async Task<bool> DeleteAsync(Guid id)
        {
            const string query = "DELETE FROM FileMessages WHERE Id = @Id";

            await using var conn = CreateConnection();

            await conn.OpenAsync();
            using (var command = new NpgsqlCommand(query, conn))
            {
                command.Parameters.AddWithValue("@Id", id);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }
        

        public async Task<List<FileMessage>> GetAllAsync()
        {
            const string query = @"
                SELECT Id, ChatId, SenderId, FileName, FilePath, FileType, FileSize, CreatedAt
                FROM FileMessages
                ORDER BY CreatedAt DESC";

            var fileMessages = new List<FileMessage>();

            await using var conn = CreateConnection();

            await conn.OpenAsync();
            using (var command = new NpgsqlCommand(query, conn))
            {
                using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            fileMessages.Add(MapFileMessage(reader));
                        }
                }
            }
            

            return fileMessages;
        }

        private FileMessage MapFileMessage(IDataReader reader)
        {
            return new FileMessage
            {
                Id = (Guid)reader["Id"],
                ChatId = (Guid)reader["ChatId"],
                SenderId = (Guid)reader["SenderId"],
                FileName = reader["FileName"].ToString(),
                FilePath = reader["FilePath"].ToString(),
                FileType = reader["FileType"].ToString(),
                FileSize = (long)reader["FileSize"],
                CreatedAt = (DateTime)reader["CreatedAt"],
            };
        }
    }
}
