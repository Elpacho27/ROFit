
namespace ROFit.Model
{
    public class MealDto
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }        
        public TimeSpan? Time { get; set; }        
    }
}
