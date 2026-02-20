import baseApi from "../BaseApi";
const ExerciseService = {
  createExercise: (exerciseDto) => baseApi.post(`/Exercise`, exerciseDto),
  deleteExercise: (exerciseId) => baseApi.delete(`/Exercise/${exerciseId}`),
};

export default ExerciseService;
