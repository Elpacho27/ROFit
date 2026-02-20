import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "../../styles/meal_plans/MealListPage.css";
import MealListStore from "../../stores/MealListStore";
import DefaultButton from "../../components/common/DefaultButton";

function MealListPage() {
  const { mealPlanId } = useParams();
  const navigate = useNavigate();

  const [meals, setMeals] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!mealPlanId) {
      setError("Missing meal plan id in URL");
      setLoading(false);
      return;
    }

    (async () => {
      try {
        setLoading(true);
        const data = await MealListStore.getMealsForMealPlan(mealPlanId);
        setMeals(data || []);
        setError(null);
      } catch (err) {
        console.error(err);
        setError("Failed to load meals");
      } finally {
        setLoading(false);
      }
    })();
  }, [mealPlanId]);

  if (loading) {
    return <p className="meals-loading">Loading meals...</p>;
  }

  if (error) {
    return <p className="meals-error">{error}</p>;
  }

  return (
    <div className="meals-page">
      <div className="meals-header">
        <div>
          <h2 className="meals-title">Meals</h2>
          <p className="meals-subtitle">All meals in this meal plan</p>
        </div>

        <div className="meals-header-buttons">
          <DefaultButton
            type="back"
            title="Go Back"
            onClick={() => navigate(-1)}
          ></DefaultButton>
        </div>
      </div>

      {meals.length === 0 ? (
        <div className="meals-empty">
          <p>No meals in this meal plan.</p>
        </div>
      ) : (
        <div className="meals-list">
          {meals.map((meal) => (
            <div
              key={meal.mealId}
              className="meal-card"
              onClick={() => navigate(`/meals/${meal.mealId}/food`)}
            >
              <div className="meal-card-header">
                <div className="meal-card-main">
                  <h3 className="meal-name">{meal.mealName}</h3>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default MealListPage;
