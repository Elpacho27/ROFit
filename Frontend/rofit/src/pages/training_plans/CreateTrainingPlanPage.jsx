import { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "../../styles/training_plans/CreateTrainingPlan.css";
import DefaultButton from "../../components/common/DefaultButton";
import CreateTrainingPlanStore from "../../stores/CreateTrainingPlanStore";

function CreateTrainingPlanPage() {
  const { userId } = useParams();
  const navigate = useNavigate();

  const [plan, setPlan] = useState({
    name: "",
    description: "",
    muscleGroup: "",
  });
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState(null);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setPlan((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSaving(true);
    setError(null);

    const dto = {
      name: plan.name,
      description: plan.description,
      muscleGroup: plan.muscleGroup,
      userId,
    };

    try {
      await CreateTrainingPlanStore.createTrainingPlan(dto);
      navigate(`/users/${userId}/training-plans`);
    } catch (err) {
      console.error(err);
      setError("Failed to create training plan");
    } finally {
      setSaving(false);
    }
  };

  return (
    <form className="plan-create-form" onSubmit={handleSubmit}>
      <div className="plan-exercises-header">
        <div>
          <h2 className="plan-exercises-title">Create Training Plan</h2>
        </div>
        <div className="plan-exercises-header-buttons">
          <DefaultButton
            title="Go Back"
            type="back"
            onClick={() => navigate(-1)}
          ></DefaultButton>
        </div>
      </div>

      {error && <p className="plan-create-error">{error}</p>}

      <div className="plan-create-field">
        <label>Name</label>
        <input name="name" value={plan.name} onChange={handleChange} required />
      </div>

      <div className="plan-create-field">
        <label>Muscle group</label>
        <input
          name="muscleGroup"
          value={plan.muscleGroup}
          onChange={handleChange}
        />
      </div>

      <div className="plan-create-field">
        <label>Description</label>
        <textarea
          name="description"
          value={plan.description}
          onChange={handleChange}
        />
      </div>

      <DefaultButton
        title={saving ? "Creating..." : "Create plan"}
        disabled={saving}
        type="submit"
      />
    </form>
  );
}

export default CreateTrainingPlanPage;
