import MealPlanMealService from "../api/services/MealPlanMealService";

class MealListStore {
  constructor() {
    this.getMealsForMealPlan = this.getMealsForMealPlan.bind(this);
  }

  async getMealsForMealPlan(mealPlanId) {
    const response = await MealPlanMealService.getMealsForMealPlan(mealPlanId);
    return response.data;
  }
}

export default new MealListStore();
