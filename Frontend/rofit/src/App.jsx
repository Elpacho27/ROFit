import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

import {
  LoginPage,
  TrainingPlanListPage,
  UserPage,
  AppLayout,
  CoachAssignmentListPage,
  ChatsListPage,
  ChatPage,
  TrainingPlanExerciseListPage,
  CreateTrainingPlanPage,
  SelectionPage,
  MealPlanListPage,
  MealListPage,
  MealFoodPage,
  HomePage,
} from "./pages";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />

        <Route element={<AppLayout />}>
          <Route index element={<HomePage />} />
          <Route path="/" element={<HomePage />} />

          <Route path="/selection-page/:userId" element={<SelectionPage />} />
          <Route
            path="/users/:userId/training-plans"
            element={<TrainingPlanListPage />}
          />
          <Route path="/chats" element={<ChatsListPage />} />
          <Route path="/chat/:chatId" element={<ChatPage />} />
          <Route path="/user" element={<UserPage />} />
          <Route
            path="/coach-assignments"
            element={<CoachAssignmentListPage />}
          />
          <Route
            path="/training-plans/:userId/:trainingPlanId"
            element={<TrainingPlanExerciseListPage />}
          />
          <Route
            path="/training-plans/:userId/create"
            element={<CreateTrainingPlanPage />}
          />
          <Route path="/meal-plans/:userId" element={<MealPlanListPage />} />
          <Route
            path="/meal-plans/:mealPlanId/meals"
            element={<MealListPage />}
          />
          <Route path="/meals/:mealId/food" element={<MealFoodPage />} />
        </Route>

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
