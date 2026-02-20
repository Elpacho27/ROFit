import baseApi from "../BaseApi";
const MealPlanService = {
  getMealPlansForUser: (userId, role) =>
    baseApi.get(`/MealPlan/user/${userId}`, {
      params: { role },
    }),
};

export default MealPlanService;
