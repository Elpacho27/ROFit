using Npgsql;
using NpgsqlTypes;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Models;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;
using System.Data;
using System.Security.Cryptography;
using System.Text;


namespace ROFit.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetByEmailAsync(string email)
        {
            User user = new User();

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(UserSqlTemplates.GET_USER_BY_EMAIL, connection))
                {
                    command.Parameters.Add("@Email", NpgsqlDbType.Text).Value = email;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = MapUserFromReader(reader);
                        }
                    }
                }
            }

            return user;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            User user = new User();

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(UserSqlTemplates.GET_USER_BY_ID, connection))
                {
                    command.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = id;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = MapUserFromReader(reader);
                        }
                    }
                }
            }

            return user;
        }

        public async Task<User> CreateAsync(UserRegister userRegister, Guid? createdBy = null)
        {
            string passwordHash = HashPassword(userRegister.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = userRegister.FullName,
                Email = userRegister.Email,
                PasswordHash = passwordHash,
                Role = userRegister.Role,
                DateCreated = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                CreatedBy = createdBy,
                IsActive = true
            };

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(UserSqlTemplates.INSERT_USER, connection))
                {
                    AddUserInsertParameters(command, user);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user.Id = reader.GetGuid("id");
                            user.DateCreated = reader.GetDateTime("datecreated");
                        }
                    }
                }
            }

            return user;
        }

        public async Task<User> UpdateAsync(User user, Guid updatedBy)
        {
            user.DateUpdated = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            user.UpdatedBy = updatedBy;

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(UserSqlTemplates.UPDATE_USER, connection))
                {
                    command.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = user.Id;
                    command.Parameters.Add("@FullName", NpgsqlDbType.Text).Value = user.FullName;
                    command.Parameters.Add("@Email", NpgsqlDbType.Text).Value = user.Email;
                    command.Parameters.Add("@Role", NpgsqlDbType.Text).Value = user.Role;
                    command.Parameters.Add("@DateUpdated", NpgsqlDbType.Timestamp).Value = user.DateUpdated;
                    command.Parameters.Add("@UpdatedBy", NpgsqlDbType.Uuid).Value = user.UpdatedBy;

                    await command.ExecuteNonQueryAsync();
                }
            }

            return user;
        }

        public async Task<bool> DeleteAsync(Guid id, Guid updatedBy)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(UserSqlTemplates.DELETE_USER, connection))
                {
                    command.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = id;
                    command.Parameters.Add("@DateUpdated", NpgsqlDbType.Timestamp).Value = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                    command.Parameters.Add("@UpdatedBy", NpgsqlDbType.Uuid).Value = updatedBy;

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(UserSqlTemplates.USER_EXISTS_BY_EMAIL, connection))
                {
                    command.Parameters.Add("@Email", NpgsqlDbType.Text).Value = email;

                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result) > 0;
                }
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();

            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(UserSqlTemplates.GET_ALL_USERS, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(MapUserFromReader(reader));
                        }
                    }
                }
            }

            return users;
        }

        public async Task<string> GetPinHashAsync(Guid userId)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("SELECT pin_hash FROM users WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", userId);
                    var result = await command.ExecuteScalarAsync();
                    return result as string;
                }
            }
        }

        public async Task SetPinAsync(Guid userId, string pin)
        {
            var pinHash = HashPin(pin);
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("UPDATE users SET pin_hash = @PinHash WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@PinHash", pinHash ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Id", userId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> VerifyPinAsync(Guid userId, string pin)
        {
            var storedHash = await GetPinHashAsync(userId);
            if (string.IsNullOrEmpty(storedHash)) return false;
            var inputHash = HashPin(pin);
            return storedHash == inputHash;
        }

        public async Task<bool> HasPinAsync(Guid userId)
        {
            var storedHash = await GetPinHashAsync(userId);
            return !string.IsNullOrEmpty(storedHash);
        }

        private string HashPin(string pin)
        {
            using (var sha256 = SHA256.Create())
            {
                var pinBytes = Encoding.UTF8.GetBytes(pin + "YourPinSaltHere!"); 
                var hashBytes = sha256.ComputeHash(pinBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }


        public bool VerifyPassword(string password, string hash)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }

        #region Private Helper Methods

        private User MapUserFromReader(NpgsqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetGuid("id"),
                FullName = reader.GetString("fullname"),
                Email = reader.GetString("email"),
                PasswordHash = reader.GetString("passwordhash"),
                Role = reader.GetString("role"),
                DateCreated = reader.GetDateTime("datecreated"),
                DateUpdated = reader.IsDBNull("dateupdated") ? null : reader.GetDateTime("dateupdated"),
                CreatedBy = reader.IsDBNull("createdby") ? null : reader.GetGuid("createdby"),
                UpdatedBy = reader.IsDBNull("updatedby") ? null : reader.GetGuid("updatedby"),
                IsActive = reader.GetBoolean("isactive")
            };
        }

        private void AddUserInsertParameters(NpgsqlCommand command, User user)
        {
            command.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = user.Id;
            command.Parameters.Add("@FullName", NpgsqlDbType.Text).Value = user.FullName;
            command.Parameters.Add("@Email", NpgsqlDbType.Text).Value = user.Email;
            command.Parameters.Add("@PasswordHash", NpgsqlDbType.Text).Value = user.PasswordHash;
            command.Parameters.Add("@Role", NpgsqlDbType.Text).Value = user.Role;
            command.Parameters.Add("@DateCreated", NpgsqlDbType.Timestamp).Value = user.DateCreated;
            command.Parameters.Add("@DateUpdated", NpgsqlDbType.Timestamp).Value = (object)user.DateUpdated ?? DBNull.Value;
            command.Parameters.Add("@CreatedBy", NpgsqlDbType.Uuid).Value = (object)user.CreatedBy ?? DBNull.Value;
            command.Parameters.Add("@UpdatedBy", NpgsqlDbType.Uuid).Value = (object)user.UpdatedBy ?? DBNull.Value;
            command.Parameters.Add("@IsActive", NpgsqlDbType.Boolean).Value = user.IsActive;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + "YourSaltHere"));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public async Task<bool> UpdateUserFcmTokens(Guid userId, string token)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(UserSqlTemplates.INSERT_FCM_TOKEN, connection))
                {
                    command.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = userId;
                    command.Parameters.Add("@FcmToken", NpgsqlDbType.Text).Value = token;

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<List<string>> GetAllUserFcmTokens(Guid userId)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(UserSqlTemplates.GET_USER_FCM_TOKENS, connection))
                {
                    command.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = userId;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var tokens = reader["fcm_tokens"] as string[];

                            return tokens != null ? tokens.ToList() : new List<string>();
                        }
                        else
                        {
                            return new List<string>();
                        }
                    }
                }
            }
        }

        public async Task<bool> DeleteUserFcmToken(Guid userId, string token)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(UserSqlTemplates.DELETE_FCM_TOKEN, connection))
                {
                    command.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = userId;
                    command.Parameters.Add("@FcmToken", NpgsqlDbType.Text).Value = token;

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        #endregion
    }
}
