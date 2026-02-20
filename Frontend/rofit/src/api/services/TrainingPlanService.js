import baseApi from "../BaseApi";
const TrainingPlanService = {
  getPlansForUser: (userId) => baseApi.get(`/TrainingPlan/user/${userId}`),
  create: (trainingPlanDto) => baseApi.post("/TrainingPlan", trainingPlanDto),
  delete: (trainingPlanId) => baseApi.delete(`/TrainingPlan/${trainingPlanId}`),
};

export default TrainingPlanService;
