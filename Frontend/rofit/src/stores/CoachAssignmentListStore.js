import CoachAssignmentsService from "../api/services/CoachAssignmentsService";
import UserService from "../api/services/UserService";

class CoachAssignmentListStore {
  constructor() {
    this.getAssignmentsByCoach = this.getAssignmentsByCoach.bind(this);
    this.getAssignmentsWithUsersByCoach =
      this.getAssignmentsWithUsersByCoach.bind(this);
  }
  async getAssignmentsByCoach(coachId) {
    const response = await CoachAssignmentsService.getByCoach(coachId);
    return response.data;
  }

  async getAssignmentsWithUsersByCoach(coachId) {
    const assignments = await this.getAssignmentsByCoach(coachId);

    const assignmentsWithUsers = await Promise.all(
      assignments.map(async (assignment) => {
        const userResponse = await UserService.getById(assignment.userId);
        return {
          ...assignment,
          user: userResponse.data,
        };
      }),
    );

    return assignmentsWithUsers;
  }
}

export default new CoachAssignmentListStore();
