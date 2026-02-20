import baseApi from "../BaseApi";
const MealFoodService = {
  getFoodForMeal: (mealId) => baseApi.get(`/FoodMeal/meal/${mealId}`),
};

export default MealFoodService;
