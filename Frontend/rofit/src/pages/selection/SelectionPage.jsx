import { useParams, useNavigate } from "react-router-dom";
import "../../styles/selection/SelectionPage.css";

function SelectionPage() {
  const { userId } = useParams();
  const navigate = useNavigate();

  if (!userId) {
    return <div>Invalid user</div>;
  }

  return (
    <div className="selection-page">
      <div className="selection-cards">
        <div
          className="selection-card"
          onClick={() => navigate(`/meal-plans/${userId}`)}
        >
          <div className="selection-card-body">
            <h3 className="selection-card-title">Nutrition</h3>
            <p className="selection-card-text">
              Track meals and optimise daily intake.
            </p>
          </div>
          <span className="selection-pill">NUTRITION</span>
        </div>

        <div
          className="selection-card"
          onClick={() => navigate(`/users/${userId}/training-plans`)}
        >
          <div className="selection-card-body">
            <h3 className="selection-card-title">Training Progress</h3>
            <p className="selection-card-text">
              View and update training plans and sessions.
            </p>
          </div>
          <span className="selection-pill">TRAINING</span>
        </div>
      </div>
    </div>
  );
}

export default SelectionPage;
