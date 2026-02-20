using ROFit.Model;

namespace ROFit.Service.Common
{
    public interface ICoachAssignmentService
    {
        Task<CoachAssignmentDto> GetByIdAsync(Guid id);
        Task<IEnumerable<CoachAssignmentDto>> GetByCoachAsync(Guid coachId);
        Task<IEnumerable<CoachAssignmentDto>> GetByUserAsync(Guid userId);
        Task<Guid> AssignUserToCoachAsync(CoachAssignmentDto assignment);
        Task<bool> UpdateAsync(CoachAssignmentDto assignment);
        Task<bool> RemoveUserFromCoachAsync(Guid id, DateTime endDate, string updatedBy);
        Task<IEnumerable<CoachAssignmentDto>> GetAllActiveAsync();
    }
}
