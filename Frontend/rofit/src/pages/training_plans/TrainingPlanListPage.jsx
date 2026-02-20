import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "../../styles/training_plans/TrainingPlanList.css";
import DefaultButton from "../../components/common/DefaultButton";
import TrainingPlanStore from "../../stores/TrainingPlanStore";

function TrainingPlanListPage() {
  const { userId } = useParams();
  const navigate = useNavigate();
  const [plans, setPlans] = useState([]);
  const [userName, setUserName] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [trainingPlanToDelete, setTrainingPlanToDelete] = useState(null);
  const [deleteConfirmOpen, setDeleteConfirmOpen] = useState(false);

  const handleViewDetails = (planId) => {
    navigate(`/training-plans/${userId}/${planId}`);
  };

  const handleCreatePlan = () => {
    navigate(`/training-plans/${userId}/create`);
  };

  const confirmDelete = async () => {
    if (!trainingPlanToDelete) return;

    try {
      const updatedPlans = await TrainingPlanStore.deleteTrainingPlan(
        userId,
        trainingPlanToDelete.id,
      );
      setPlans(updatedPlans);
      setDeleteConfirmOpen(false);
      setTrainingPlanToDelete(null);
    } catch (err) {
      console.error(err);
      alert("Failed to delete exercise");
    }
  };

  const cancelDelete = () => {
    setDeleteConfirmOpen(false);
    setTrainingPlanToDelete(null);
  };

  useEffect(() => {
    if (!userId) {
      setError("Missing userId in URL");
      setLoading(false);
      return;
    }

    (async () => {
      try {
        const { plans, userName } =
          await TrainingPlanStore.loadTrainingPlansPage(userId);

        setPlans(plans);
        setUserName(userName);
      } catch (err) {
        console.error(err);
        setError("Failed to load training plans");
      } finally {
        setLoading(false);
      }
    })();
  }, [userId]);

  if (loading)
    return <p className="training-plans-loading">Loading training plans...</p>;
  if (error) return <p className="training-plans-error">{error}</p>;

  return (
    <div className="training-plans-page">
      <div className="training-plans-header">
        <div>
          <h2 className="training-plans-title">Training Plans</h2>
          <p className="training-plans-subtitle">
            {userName} — {plans.length} {plans.length === 1 ? "plan" : "plans"}
          </p>
        </div>
        <div className="training-plan-header-buttons">
          <DefaultButton
            title="Go Back"
            type="back"
            onClick={() => navigate(-1)}
          ></DefaultButton>
          <DefaultButton title="+ New plan" onClick={handleCreatePlan} />
        </div>
      </div>

      {plans.length === 0 ? (
        <div className="training-plans-empty">
          <p>No training plans yet.</p>
        </div>
      ) : (
        <div className="training-plans-list">
          {plans.map((plan) => (
            <div key={plan.id} className="training-plan-card">
              <div className="training-plan-header">
                <h3 className="training-plan-name">{plan.name}</h3>
                <div style={{ display: "flex", gap: "20px" }}>
                  <span className="training-plan-muscle">
                    {plan.muscleGroup}
                  </span>
                  <button
                    type="button"
                    className="plan-exercise-delete"
                    onClick={() => {
                      setTrainingPlanToDelete({
                        id: plan.id,
                        name: plan.name,
                      });
                      setDeleteConfirmOpen(true);
                    }}
                  >
                    ✕
                  </button>
                </div>
              </div>
              <p className="training-plan-description">
                {plan.description || "No description provided."}
              </p>
              <DefaultButton
                title="View Details →"
                onClick={() => handleViewDetails(plan.id)}
              ></DefaultButton>
            </div>
          ))}
        </div>
      )}
      {deleteConfirmOpen && trainingPlanToDelete && (
        <div className="tp-modal-overlay">
          <div className="tp-modal tp-modal--small">
            <div className="tp-modal-header">
              <h3>Remove training plan</h3>
              <button type="button" onClick={cancelDelete}>
                ✕
              </button>
            </div>

            <p className="tp-modal-text">
              Are you sure you want to remove{" "}
              <span className="tp-modal-text-strong">
                {trainingPlanToDelete.name || "this training plan"}
              </span>{" "}
              ?
            </p>

            <div className="tp-modal-actions tp-modal-actions--confirm">
              <DefaultButton
                title="Cancel"
                type="back"
                onClick={cancelDelete}
              ></DefaultButton>
              <DefaultButton
                title="Remove"
                onClick={confirmDelete}
              ></DefaultButton>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

export default TrainingPlanListPage;
