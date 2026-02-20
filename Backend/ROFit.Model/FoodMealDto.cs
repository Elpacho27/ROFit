
namespace ROFit.Model
{
    public class FoodMealDto
    {
        public Guid? Id { get; set; }
        public Guid? MealId { get; set; }
        public int? FoodId { get; set; }
        public decimal Grams { get; set; }
    }
}
