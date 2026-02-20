using ROFit.Model;
using ROFit.Repository.Common;
using ROFit.Service.Common;

namespace ROFit.Service
{
    public class CoachAssignmentService : ICoachAssignmentService
    {
        private readonly ICoachAssignmentRepository _repo;

        public CoachAssignmentService(ICoachAssignmentRepository repo)
        {
            _repo = repo;
        }

        public Task<CoachAssignmentDto> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

        public Task<IEnumerable<CoachAssignmentDto>> GetByCoachAsync(Guid coachId) => _repo.GetByCoachAsync(coachId);

        public Task<IEnumerable<CoachAssignmentDto>> GetByUserAsync(Guid userId) => _repo.GetByUserAsync(userId);

        public Task<Guid> AssignUserToCoachAsync(CoachAssignmentDto assignment) => _repo.InsertAsync(assignment);

        public Task<bool> UpdateAsync(CoachAssignmentDto assignment) => _repo.UpdateAsync(assignment);

        public Task<bool> RemoveUserFromCoachAsync(Guid id, DateTime endDate, string updatedBy) =>
            _repo.RemoveAsync(id, endDate, updatedBy);

        public Task<IEnumerable<CoachAssignmentDto>> GetAllActiveAsync() => _repo.GetAllActiveAsync();
    }
}
