using Microsoft.Extensions.Configuration;
using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;

namespace ROFit.Repository
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        public ChatMessageRepository(IConfiguration config)
        {
        }

        private NpgsqlConnection CreateConnection() => ConnectionFactory.CreateConnection();

        public async Task<IReadOnlyList<ChatMessage>> GetMessagesAsync(Guid chatId, int take, int skip)
        {
            const string sql = @"
            SELECT id, chat_id, sender_id, content, created_at, is_read
            FROM chat_messages
            WHERE chat_id = @chatId
            ORDER BY created_at DESC
            LIMIT @take OFFSET @skip;";

            var result = new List<ChatMessage>();

            await using var conn = CreateConnection();
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("chatId", chatId);
            cmd.Parameters.AddWithValue("take", take);
            cmd.Parameters.AddWithValue("skip", skip);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new ChatMessage
                {
                    Id = reader.GetGuid(0),
                    ChatId = reader.GetGuid(1),
                    SenderId = reader.GetGuid(2),
                    Content = reader.GetString(3),
                    CreatedAt = reader.GetDateTime(4),
                    IsRead = reader.GetBoolean(5)
                });
            }

            return result;
        }

        public async Task<ChatMessage> AddAsync(ChatMessage message)
        {
            const string sql = @"
            INSERT INTO chat_messages (id, chat_id, sender_id, content, created_at, is_read)
            VALUES (@id, @chatId, @senderId, @content, @createdAt, @isRead);";

            if (message.Id == Guid.Empty)
                message.Id = Guid.NewGuid();
            if (message.CreatedAt == default)
                message.CreatedAt = DateTime.UtcNow;

            await using var conn = CreateConnection();
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", message.Id);
            cmd.Parameters.AddWithValue("chatId", message.ChatId);
            cmd.Parameters.AddWithValue("senderId", message.SenderId);
            cmd.Parameters.AddWithValue("content", message.Content);
            cmd.Parameters.AddWithValue("createdAt", message.CreatedAt);
            cmd.Parameters.AddWithValue("isRead", message.IsRead);

            await cmd.ExecuteNonQueryAsync();
            return message;
        }
    }

}
