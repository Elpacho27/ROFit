
using Npgsql;
using Microsoft.Extensions.Configuration;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.DAL;

public class ChatRepository : IChatRepository
{

    public ChatRepository(IConfiguration config)
    {
    }

    private NpgsqlConnection CreateConnection() => ConnectionFactory.CreateConnection();

    public async Task<Chat?> GetByIdAsync(Guid chatId)
    {
        const string sql = @"
            SELECT id, coach_id, client_id, created_at
            FROM chats
            WHERE id = @id";

        await using var conn = CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", chatId);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new Chat
        {
            Id = reader.GetGuid(0),
            CoachId = reader.GetGuid(1),
            ClientId = reader.GetGuid(2),
            CreatedAt = reader.GetDateTime(3)
        };
    }

    public async Task<Chat?> GetByCoachClientAsync(Guid coachId, Guid clientId)
    {
        const string sql = @"
            SELECT id, coach_id, client_id, created_at
            FROM chats
            WHERE coach_id = @coachId AND client_id = @clientId";

        await using var conn = CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("coachId", coachId);
        cmd.Parameters.AddWithValue("clientId", clientId);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new Chat
        {
            Id = reader.GetGuid(0),
            CoachId = reader.GetGuid(1),
            ClientId = reader.GetGuid(2),
            CreatedAt = reader.GetDateTime(3)
        };
    }

    public async Task<Chat> AddAsync(Chat chat)
    {
        const string sql = @"
            INSERT INTO chats (id, coach_id, client_id, created_at)
            VALUES (@id, @coachId, @clientId, @createdAt);";

        if (chat.Id == Guid.Empty)
            chat.Id = Guid.NewGuid();
        if (chat.CreatedAt == default)
            chat.CreatedAt = DateTime.UtcNow;

        await using var conn = CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", chat.Id);
        cmd.Parameters.AddWithValue("coachId", chat.CoachId);
        cmd.Parameters.AddWithValue("clientId", chat.ClientId);
        cmd.Parameters.AddWithValue("createdAt", chat.CreatedAt);

        await cmd.ExecuteNonQueryAsync();
        return chat;
    }

    public async Task<IReadOnlyList<Chat>> GetForUserAsync(Guid userId)
    {
        const string sql = @"
            SELECT id, coach_id, client_id, created_at
            FROM chats
            WHERE coach_id = @userId OR client_id = @userId
            ORDER BY created_at DESC;";

        var result = new List<Chat>();

        await using var conn = CreateConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("userId", userId);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new Chat
            {
                Id = reader.GetGuid(0),
                CoachId = reader.GetGuid(1),
                ClientId = reader.GetGuid(2),
                CreatedAt = reader.GetDateTime(3)
            });
        }

        return result;
    }
}
