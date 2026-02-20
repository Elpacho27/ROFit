import baseApi from "../BaseApi";
const TrainingPlanExerciseService = {
  getTrainingPlanExercises: (userId, trainingPlanId) =>
    baseApi.get(`/TrainingPlanExercise/user/${userId}/plan/${trainingPlanId}`),
  createTrainingPlanExercise: (trainingPlanExerciseDto) =>
    baseApi.post(`/TrainingPlanExercise`, trainingPlanExerciseDto),
  deleteTrainingPlanExercisesFromPlan: (
    userId,
    trainingPlanId,
    exerciseId,
    dayOfWeek,
  ) =>
    baseApi.delete(
      `/TrainingPlanExercise/${userId}/${trainingPlanId}/${exerciseId}/${dayOfWeek}`,
    ),
};

export default TrainingPlanExerciseService;
