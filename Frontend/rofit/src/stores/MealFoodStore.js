import MealFoodService from "../api/services/MealFoodService";

class MealFoodStore {
  constructor() {
    this.getFoodForMeal = this.getFoodForMeal.bind(this);
  }

  async getFoodForMeal(mealId) {
    const response = await MealFoodService.getFoodForMeal(mealId);
    return response.data;
  }
}

export default new MealFoodStore();
