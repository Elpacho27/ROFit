
namespace ROFit.Model
{
    public class CoachAssignmentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CoachId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}

