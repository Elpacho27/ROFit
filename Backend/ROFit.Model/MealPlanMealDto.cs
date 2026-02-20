
namespace ROFit.Model
{
    public class MealPlanMealDto
    {
        public Guid? Id { get; set; }
        public Guid MealPlanId { get; set; }
        public Guid MealId { get; set; }
        public short? MealOrder { get; set; }
        public string? MealName { get; set; }
        public DateTime? MealDate { get; set; }
        public TimeSpan? MealTime { get; set; }
    }
}
