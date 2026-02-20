import TrainingPlanService from "../api/services/TrainingPlanService";

class CreateTrainingPlanStore {
  constructor() {
    this.createTrainingPlan = this.createTrainingPlan.bind(this);
  }
  async createTrainingPlan(dto) {
    return await TrainingPlanService.create(dto);
  }
}

export default new CreateTrainingPlanStore();
