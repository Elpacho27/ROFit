
namespace ROFit.Model
{
    public class FoodMealWithFoodDto
    {
        public Guid Id { get; set; }
        public Guid MealId { get; set; }
        public int FoodId { get; set; }
        public decimal Grams { get; set; }
        public string Name { get; set; }
        public decimal CaloriesPer100g { get; set; }
        public decimal ProteinPer100g { get; set; }
        public decimal CarbsPer100g { get; set; }
        public decimal FatPer100g { get; set; }
    }
}
