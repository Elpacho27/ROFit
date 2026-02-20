import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "../../styles/meal_plans/MealPlanListPage.css";
import MealPlanListStore from "../../stores/MealPlanListStore";
import DefaultButton from "../../components/common/DefaultButton";

function MealPlanListPage() {
  const { userId } = useParams();
  const navigate = useNavigate();

  const [mealPlans, setMealPlans] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!userId) {
      setError("Missing user id in URL");
      setLoading(false);
      return;
    }

    (async () => {
      try {
        setLoading(true);
        const response = await MealPlanListStore.getMealPlansForUser(
          userId,
          "Coach",
        );
        const plans = response.data || response;
        setMealPlans(plans);
        console.log(plans);
        setError(null);
      } catch (err) {
        console.error(err);
        setError("Failed to load meal plans");
      } finally {
        setLoading(false);
      }
    })();
  }, [userId]);

  if (loading) {
    return <p className="meal-plans-loading">Loading meal plans...</p>;
  }

  if (error) {
    return <p className="meal-plans-error">{error}</p>;
  }

  return (
    <div className="meal-plans-page">
      <div className="meal-plans-header">
        <div>
          <h2 className="meal-plans-title">Meal Plans</h2>
          <p className="meal-plans-subtitle">
            Overview of all meal plans for this user
          </p>
        </div>
        <div className="meal-plans-header-buttons">
          <DefaultButton
            type="back"
            title="Go Back"
            onClick={() => navigate(-1)}
          ></DefaultButton>
        </div>
      </div>

      {mealPlans.length === 0 ? (
        <div className="meal-plans-empty">
          <p>No meal plans found.</p>
        </div>
      ) : (
        <div className="meal-plans-list">
          {mealPlans.map((plan) => (
            <div
              key={plan.id}
              className="meal-plan-card"
              onClick={() => navigate(`/meal-plans/${plan.id}/meals`)}
            >
              <div className="meal-plan-card-header">
                <div className="meal-plan-card-main">
                  <h3 className="meal-plan-name">{plan.name}</h3>
                  <p className="meal-plan-description">
                    {plan.description || "No description provided."}
                  </p>
                </div>

                {plan.target && (
                  <span className="meal-plan-pill">
                    {plan.target.toUpperCase()}
                  </span>
                )}
              </div>

              <div className="meal-plan-footer">
                <span className="meal-plan-meta">
                  Total calories: {plan.caloriesLimit ?? "-"}
                </span>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default MealPlanListPage;
