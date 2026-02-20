using Npgsql;
using ROFit.DAL;
using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Repository.SqlTemplates;

namespace ROFit.Repository
{
    public class CoachAssignmentRepository : ICoachAssignmentRepository
    {
        public async Task<CoachAssignmentDto> GetByIdAsync(Guid id)
        {
            CoachAssignmentDto assignment = null;
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(CoachAssignmentSqlTemplates.GET_ASSIGNMENT_BY_ID, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            assignment = MapFromReader(reader);
                        }
                    }
                }
            }
            return assignment;
        }

        public async Task<IEnumerable<CoachAssignmentDto>> GetByCoachAsync(Guid coachId)
        {
            var result = new List<CoachAssignmentDto>();
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(CoachAssignmentSqlTemplates.GET_ASSIGNMENTS_BY_COACH, connection))
                {
                    command.Parameters.AddWithValue("@CoachId", coachId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(MapFromReader(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<CoachAssignmentDto>> GetByUserAsync(Guid userId)
        {
            var result = new List<CoachAssignmentDto>();
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(CoachAssignmentSqlTemplates.GET_ASSIGNMENTS_BY_USER, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(MapFromReader(reader));
                        }
                    }
                }
            }
            return result;
        }

        public async Task<Guid> InsertAsync(CoachAssignmentDto dto)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(CoachAssignmentSqlTemplates.ASSIGN_USER_TO_COACH, connection))
                {
                    command.Parameters.AddWithValue("@Id", dto.Id);
                    command.Parameters.AddWithValue("@UserId", dto.UserId);
                    command.Parameters.AddWithValue("@CoachId", dto.CoachId);
                    command.Parameters.AddWithValue("@StartDate", dto.StartDate);
                    command.Parameters.AddWithValue("@EndDate", (object)dto.EndDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateCreated", dto.DateCreated);
                    command.Parameters.AddWithValue("@DateUpdated", (object)dto.DateUpdated ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedBy", dto.CreatedBy ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UpdatedBy", dto.UpdatedBy ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", dto.IsActive);

                    var idObj = await command.ExecuteScalarAsync();
                    return (Guid)idObj;
                }
            }
        }

        public async Task<bool> UpdateAsync(CoachAssignmentDto dto)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(CoachAssignmentSqlTemplates.UPDATE_ASSIGNMENT, connection))
                {
                    command.Parameters.AddWithValue("@Id", dto.Id);
                    command.Parameters.AddWithValue("@StartDate", dto.StartDate);
                    command.Parameters.AddWithValue("@EndDate", (object)dto.EndDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateUpdated", dto.DateUpdated ?? DateTime.UtcNow);
                    command.Parameters.AddWithValue("@UpdatedBy", dto.UpdatedBy ?? (object)DBNull.Value);

                    var affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }

        public async Task<bool> RemoveAsync(Guid id, DateTime endDate, string updatedBy)
        {
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(CoachAssignmentSqlTemplates.REMOVE_USER_FROM_COACH, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    command.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);
                    command.Parameters.AddWithValue("@UpdatedBy", updatedBy);

                    var affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }

        public async Task<IEnumerable<CoachAssignmentDto>> GetAllActiveAsync()
        {
            var list = new List<CoachAssignmentDto>();
            using (var connection = ConnectionFactory.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(CoachAssignmentSqlTemplates.GET_ALL_ACTIVE_ASSIGNMENTS, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(MapFromReader(reader));
                        }
                    }
                }
            }
            return list;
        }

        private CoachAssignmentDto MapFromReader(NpgsqlDataReader reader)
        {
            return new CoachAssignmentDto
            {
                Id = reader.GetGuid(reader.GetOrdinal("id")),
                UserId = reader.GetGuid(reader.GetOrdinal("userid")),
                CoachId = reader.GetGuid(reader.GetOrdinal("coachid")),
                StartDate = reader.GetDateTime(reader.GetOrdinal("startdate")),
                EndDate = reader.IsDBNull(reader.GetOrdinal("enddate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("enddate")),
                DateCreated = reader.GetDateTime(reader.GetOrdinal("datecreated")),
                DateUpdated = reader.IsDBNull(reader.GetOrdinal("dateupdated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dateupdated")),
                CreatedBy = reader.IsDBNull(reader.GetOrdinal("createdby")) ? null : reader.GetString(reader.GetOrdinal("createdby")),
                UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updatedby")) ? null : reader.GetString(reader.GetOrdinal("updatedby")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("isactive")),
            };
        }
    }
}
