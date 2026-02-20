
namespace ROFit.Model
{
    public class MealPlanDto
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CaloriesLimit { get; set; }
        public bool? IsVisible { get; set; }
    }
}
