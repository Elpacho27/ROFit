using ROFit.Model;

namespace ROFit.Repository.Common
{
    public interface ICoachAssignmentRepository
    {
        Task<CoachAssignmentDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<CoachAssignmentDto>> GetByCoachAsync(Guid coachId);
        Task<IEnumerable<CoachAssignmentDto>> GetByUserAsync(Guid userId);
        Task<Guid> InsertAsync(CoachAssignmentDto assignment);
        Task<bool> UpdateAsync(CoachAssignmentDto assignment);
        Task<bool> RemoveAsync(Guid id, DateTime endDate, string updatedBy);
        Task<IEnumerable<CoachAssignmentDto>> GetAllActiveAsync();
    }

}
