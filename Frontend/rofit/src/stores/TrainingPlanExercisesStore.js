import TrainingPlanExerciseService from "../api/services/TrainingPlanExerciseService";
import ExercisesService from "../api/services/ExerciseService";
import UserService from "../api/services/UserService";

class TrainingPlanExerciseStore {
  constructor() {
    this.getTrainingPlanExercises = this.getTrainingPlanExercises.bind(this);
  }

  async getTrainingPlanExercises(userId, trainingPlanId) {
    const response = await TrainingPlanExerciseService.getTrainingPlanExercises(
      userId,
      trainingPlanId,
    );
    return response.data;
  }

  async createTrainingPlanExercise(trainingPlanExerciseDto) {
    return await TrainingPlanExerciseService.createTrainingPlanExercise(
      trainingPlanExerciseDto,
    );
  }

  async createExercise(exerciseDto) {
    return await ExercisesService.createExercise(exerciseDto);
  }

  async deleteTrainingPlanExercise(
    userId,
    trainingPlanId,
    exerciseId,
    dayOfWeek,
  ) {
    return await TrainingPlanExerciseService.deleteTrainingPlanExercisesFromPlan(
      userId,
      trainingPlanId,
      exerciseId,
      dayOfWeek,
    );
  }

  async getUserById(userId) {
    return await UserService.getById(userId);
  }

  async loadPageData(userId, trainingPlanId) {
    const [exRes, userRes] = await Promise.all([
      this.getTrainingPlanExercises(userId, trainingPlanId),
      this.getUserById(userId),
    ]);

    return {
      exercises: exRes.exercises || [],
      userName: userRes.data?.fullName || "User",
    };
  }

  async createExerciseForPlan(userId, trainingPlanId, form) {
    const exerciseDto = {
      name: form.name,
      description: form.description,
      primaryMuscleGroup: form.primaryMuscleGroup,
      secondaryMuscleGroup: form.secondaryMuscleGroup
        ? form.secondaryMuscleGroup.split(",").map((s) => s.trim())
        : [],
      defaultSets: form.defaultSets ? Number(form.defaultSets) : null,
      defaultReps: form.defaultReps ? Number(form.defaultReps) : null,
      durationSeconds: form.duration ? Number(form.duration) : null,
    };

    const response = await this.createExercise(exerciseDto);
    const exerciseId = response.data.id;

    const trainingPlaneExerciseDto = {
      userId,
      trainingPlanId,
      exerciseId,
      dayOfWeek: form.dayOfWeek,
      isCompleted: false,
    };

    await this.createTrainingPlanExercise(trainingPlaneExerciseDto);

    const updated = await this.getTrainingPlanExercises(userId, trainingPlanId);
    return updated.exercises || [];
  }

  async deleteExerciseFromPlan(userId, trainingPlanId, exerciseId, dayOfWeek) {
    await this.deleteTrainingPlanExercise(
      userId,
      trainingPlanId,
      exerciseId,
      dayOfWeek,
    );
    const updated = await this.getTrainingPlanExercises(userId, trainingPlanId);
    return updated.exercises || [];
  }
  async deleteExercise(exerciseId) {
    return await ExercisesService.deleteExercise(exerciseId);
  }
}

export default new TrainingPlanExerciseStore();
