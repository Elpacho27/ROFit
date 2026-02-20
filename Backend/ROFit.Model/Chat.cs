
namespace ROFit.Model
{
    public class Chat
    {
        public Guid Id { get; set; }
        public Guid CoachId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
