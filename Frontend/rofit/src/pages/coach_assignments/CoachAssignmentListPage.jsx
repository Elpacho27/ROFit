import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import "../../styles/coach_assignments/CoachAssignmentList.css";
import CoachAssignmentListStore from "../../stores/CoachAssignmentListStore";
import CoachAssignmentCard from "../../components/coach_assignment_list/CoachAssignmentCard";
function CoachAssignmentListPage() {
  const navigate = useNavigate();
  const [assignments, setAssignments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const userJson = localStorage.getItem("user");
    if (!userJson) {
      setError("User not found in localStorage");
      setLoading(false);
      return;
    }

    const user = JSON.parse(userJson);
    const coachId = user.id;

    if (!coachId) {
      setError("coachId missing on user");
      setLoading(false);
      return;
    }

    (async () => {
      try {
        const data =
          await CoachAssignmentListStore.getAssignmentsWithUsersByCoach(
            coachId,
          );
        setAssignments(data);
      } catch (err) {
        console.error(err);
        setError("Failed to load assignments");
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="coach-assignments-root">
      <div className="coach-assignments-page">
        <div className="coach-assignments-grid">
          {assignments.map((assignment) => (
            <CoachAssignmentCard
              key={assignment.id}
              assignment={assignment}
              onClick={() => navigate(`/selection-page/${assignment.user.id}`)}
            />
          ))}
        </div>
      </div>
    </div>
  );
}

export default CoachAssignmentListPage;
