import baseApi from "../BaseApi";
const CoachAssignmentService = {
  getByCoach: (coachId) => baseApi.get(`/CoachAssignment/coach/${coachId}`),
};

export default CoachAssignmentService;
