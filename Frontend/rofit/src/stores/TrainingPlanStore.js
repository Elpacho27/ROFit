import TrainingPlanService from "../api/services/TrainingPlanService";
import TrainingPlanExerciseService from "../api/services/TrainingPlanExerciseService";
import ExerciseService from "../api/services/ExerciseService";
import UserService from "../api/services/UserService";

class TrainingPlanStore {
  async getTrainingPlansByUser(userId) {
    const response = await TrainingPlanService.getPlansForUser(userId);
    return response.data.trainingPlans || [];
  }

  async loadTrainingPlansPage(userId) {
    const [plans, userResponse] = await Promise.all([
      this.getTrainingPlansByUser(userId),
      UserService.getById(userId),
    ]);

    return {
      plans,
      userName: userResponse.data?.fullName || "User",
    };
  }

  async deleteTrainingPlan(userId, trainingPlanId) {
    const exercisesResponse =
      await TrainingPlanExerciseService.getTrainingPlanExercises(
        userId,
        trainingPlanId,
      );

    const exercises = exercisesResponse.data.exercises || [];

    for (const exercise of exercises) {
      await TrainingPlanExerciseService.deleteTrainingPlanExercisesFromPlan(
        userId,
        trainingPlanId,
        exercise.id,
        exercise.dayOfWeek,
      );

      await ExerciseService.deleteExercise(exercise.id);
    }

    await TrainingPlanService.delete(trainingPlanId);

    return await this.getTrainingPlansByUser(userId);
  }
}

export default new TrainingPlanStore();
