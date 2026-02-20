import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "../../styles/meal_plans/MealFoodPage.css";
import MealFoodStore from "../../stores/MealFoodStore";
import DefaultButton from "../../components/common/DefaultButton";

function MealFoodPage() {
  const { mealId } = useParams();
  const navigate = useNavigate();

  const [foods, setFoods] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!mealId) {
      setError("Missing meal id in URL");
      setLoading(false);
      return;
    }

    (async () => {
      try {
        setLoading(true);
        const data = await MealFoodStore.getFoodForMeal(mealId);
        setFoods(data || []);
        console.log(data);
        setError(null);
      } catch (err) {
        console.error(err);
        setError("Failed to load food for this meal");
      } finally {
        setLoading(false);
      }
    })();
  }, [mealId]);

  if (loading) {
    return <p className="meal-food-loading">Loading food...</p>;
  }

  if (error) {
    return <p className="meal-food-error">{error}</p>;
  }

  return (
    <div className="meal-food-page">
      <div className="meal-food-header">
        <div>
          <h2 className="meal-food-title">Meal food</h2>
          <p className="meal-food-subtitle">All foods included in this meal</p>
        </div>

        <div className="meal-food-header-buttons">
          <DefaultButton
            type="back"
            title="Go Back"
            onClick={() => navigate(-1)}
          ></DefaultButton>
        </div>
      </div>

      {foods.length === 0 ? (
        <div className="meal-food-empty">
          <p>No food items in this meal.</p>
        </div>
      ) : (
        <div className="meal-food-list">
          {foods.map((item) => {
            const totalCalories = (item.caloriesPer100g * item.grams) / 100;
            const totalProtein = (item.proteinPer100g * item.grams) / 100;
            const totalCarbs = (item.carbsPer100g * item.grams) / 100;
            const totalFat = (item.fatPer100g * item.grams) / 100;

            return (
              <div key={item.id} className="meal-food-card">
                <div className="meal-food-card-header">
                  <div className="meal-food-card-main">
                    <h3 className="meal-food-name">{item.name}</h3>
                    <p className="meal-food-description">{item.grams} g</p>
                  </div>

                  <span className="meal-food-pill">
                    {Math.round(totalCalories)} kcal
                  </span>
                </div>

                <div className="meal-food-footer">
                  <span className="meal-food-meta">
                    Protein: {totalProtein.toFixed(1)} g
                  </span>
                  <span className="meal-food-meta">
                    Carbs: {totalCarbs.toFixed(1)} g
                  </span>
                  <span className="meal-food-meta">
                    Fat: {totalFat.toFixed(1)} g
                  </span>
                </div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
}

export default MealFoodPage;
