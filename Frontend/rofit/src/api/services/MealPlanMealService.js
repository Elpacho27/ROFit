import baseApi from "../BaseApi";
const MealPlanMealService = {
  getMealsForMealPlan: (mealPlanId) =>
    baseApi.get(`/MealPlanMeal/meal_plan/${mealPlanId}`),
};

export default MealPlanMealService;
