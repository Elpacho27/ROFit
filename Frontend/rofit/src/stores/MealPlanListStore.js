import MealPlanService from "../api/services/MealPlanService";

class MealPlanListStore {
  constructor() {
    this.getMealPlansForUser = this.getMealPlansForUser.bind(this);
  }
  async getMealPlansForUser(userId, role) {
    return await MealPlanService.getMealPlansForUser(userId, role);
  }
}

export default new MealPlanListStore();
