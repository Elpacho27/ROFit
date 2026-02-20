import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "../../styles/training_plan_exercises/TrainingPlanExercises.css";
import TrainingPlanExercisesStore from "../../stores/TrainingPlanExercisesStore";
import DefaultButton from "../../components/common/DefaultButton";

const days = [
  { value: 1, label: "Monday" },
  { value: 2, label: "Tuesday" },
  { value: 3, label: "Wednesday" },
  { value: 4, label: "Thursday" },
  { value: 5, label: "Friday" },
  { value: 6, label: "Saturday" },
  { value: 7, label: "Sunday" },
];

const getDayLabel = (value) => {
  const d = days.find((x) => x.value === value);
  return d ? d.label : value;
};

function TrainingPlanExerciseListPage() {
  const { userId, trainingPlanId } = useParams();
  const navigate = useNavigate();

  const [exercises, setExercises] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [form, setForm] = useState({
    name: "",
    description: "",
    primaryMuscleGroup: "",
    secondaryMuscleGroup: "",
    defaultSets: "",
    defaultReps: "",
    dayOfWeek: 1,
    duration: "",
  });

  const [deleteConfirmOpen, setDeleteConfirmOpen] = useState(false);
  const [exerciseToDelete, setExerciseToDelete] = useState(null);

  const resetForm = () => {
    setForm({
      name: "",
      description: "",
      primaryMuscleGroup: "",
      secondaryMuscleGroup: "",
      defaultSets: "",
      defaultReps: "",
      dayOfWeek: 1,
      duration: "",
    });
  };

  const handleCreateExercise = () => {
    setIsModalOpen(true);
  };

  useEffect(() => {
    if (!userId || !trainingPlanId) {
      setError("Missing Id in URL");
      setLoading(false);
      return;
    }

    (async () => {
      try {
        setLoading(true);
        const data = await TrainingPlanExercisesStore.loadPageData(
          userId,
          trainingPlanId,
        );

        setExercises(data.exercises);
        setError(null);
      } catch (err) {
        console.error(err);
        setError("Failed to load exercises");
      } finally {
        setLoading(false);
      }
    })();
  }, [userId, trainingPlanId]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const updatedExercises =
        await TrainingPlanExercisesStore.createExerciseForPlan(
          userId,
          trainingPlanId,
          form,
        );

      setExercises(updatedExercises);
      setIsModalOpen(false);
      resetForm();
    } catch (err) {
      console.error(err);
      alert("Failed to create exercise");
    }
  };

  const confirmDelete = async () => {
    if (!exerciseToDelete) return;

    try {
      const updatedExercises =
        await TrainingPlanExercisesStore.deleteExerciseFromPlan(
          userId,
          trainingPlanId,
          exerciseToDelete.id,
          exerciseToDelete.dayOfWeek,
        );
      await TrainingPlanExercisesStore.deleteExercise(exerciseToDelete.id);

      setExercises(updatedExercises);
      setDeleteConfirmOpen(false);
      setExerciseToDelete(null);
    } catch (err) {
      console.error(err);
      alert("Failed to delete exercise");
    }
  };

  const cancelDelete = () => {
    setDeleteConfirmOpen(false);
    setExerciseToDelete(null);
  };

  if (loading) {
    return <p className="plan-exercises-loading">Loading exercises...</p>;
  }
  if (error) {
    return <p className="plan-exercises-error">{error}</p>;
  }

  return (
    <div className="plan-exercises-page">
      <div className="plan-exercises-header">
        <div>
          <h2 className="plan-exercises-title">Exercises</h2>
        </div>
        <div className="plan-exercises-header-buttons">
          <DefaultButton
            title="Go Back"
            type="back"
            onClick={() => navigate(-1)}
          ></DefaultButton>
          <DefaultButton title="Add Exercise" onClick={handleCreateExercise} />
        </div>
      </div>

      {exercises.length === 0 ? (
        <div className="plan-exercises-empty">
          <p>No exercises in this plan yet.</p>
        </div>
      ) : (
        <div className="plan-exercises-list">
          {exercises.map((ex) => (
            <div key={ex.id} className="plan-exercise-card">
              <div className="plan-exercise-header">
                <div className="plan-exercise-header-main">
                  <h3 className="plan-exercise-name">{ex.name}</h3>
                  <span className="plan-exercise-muscle">
                    {ex.primaryMuscleGroup}
                    {ex.secondaryMuscleGroup != ""
                      ? ` • ${ex.secondaryMuscleGroup}`
                      : ""}
                  </span>
                </div>

                <button
                  type="button"
                  className="plan-exercise-delete"
                  onClick={() => {
                    setExerciseToDelete({
                      id: ex.id,
                      dayOfWeek: ex.dayOfWeek,
                      name: ex.name,
                    });
                    setDeleteConfirmOpen(true);
                  }}
                >
                  ✕
                </button>
              </div>

              <div className="plan-exercise-meta-row">
                <span className="plan-exercise-pill">
                  Sets: {ex.defaultSets ?? "-"}
                </span>
                <span className="plan-exercise-pill">
                  Reps: {ex.defaultReps ?? "-"}
                </span>
                <span className="plan-exercise-pill">
                  Rest: {ex.durationSeconds ? `${ex.durationSeconds}s` : "-"}
                </span>
              </div>

              <div className="plan-exercise-footer">
                <span className="plan-exercise-day">
                  Day: {getDayLabel(ex.dayOfWeek)}
                </span>
                <span
                  className={`plan-exercise-status ${
                    ex.isCompleted
                      ? "plan-exercise-status--done"
                      : "plan-exercise-status--todo"
                  }`}
                >
                  {ex.isCompleted ? "Completed" : "Not completed"}
                </span>
              </div>
            </div>
          ))}
        </div>
      )}

      {isModalOpen && (
        <div className="tp-modal-overlay">
          <div className="tp-modal">
            <div className="tp-modal-header">
              <h3>Add Exercise</h3>
              <button type="button" onClick={() => setIsModalOpen(false)}>
                ✕
              </button>
            </div>

            <form onSubmit={handleSubmit} className="tp-modal-form">
              <input
                placeholder="Exercise name"
                required
                value={form.name}
                onChange={(e) => setForm({ ...form, name: e.target.value })}
              />

              <textarea
                placeholder="Description"
                value={form.description}
                onChange={(e) =>
                  setForm({ ...form, description: e.target.value })
                }
              />

              <input
                placeholder="Primary muscle group"
                required
                value={form.primaryMuscleGroup}
                onChange={(e) =>
                  setForm({
                    ...form,
                    primaryMuscleGroup: e.target.value,
                  })
                }
              />

              <input
                placeholder="Secondary muscle group"
                value={form.secondaryMuscleGroup}
                onChange={(e) =>
                  setForm({
                    ...form,
                    secondaryMuscleGroup: e.target.value,
                  })
                }
              />

              <div className="tp-modal-row">
                <input
                  type="number"
                  placeholder="Sets"
                  value={form.defaultSets}
                  onChange={(e) =>
                    setForm({ ...form, defaultSets: e.target.value })
                  }
                />
                <input
                  type="number"
                  placeholder="Reps"
                  value={form.defaultReps}
                  onChange={(e) =>
                    setForm({ ...form, defaultReps: e.target.value })
                  }
                />
                <input
                  type="number"
                  placeholder="Duration (s)"
                  value={form.duration}
                  onChange={(e) =>
                    setForm({ ...form, duration: e.target.value })
                  }
                />
              </div>

              <select
                value={form.dayOfWeek}
                onChange={(e) =>
                  setForm({
                    ...form,
                    dayOfWeek: Number(e.target.value),
                  })
                }
              >
                {days.map((d) => (
                  <option key={d.value} value={d.value}>
                    {d.label}
                  </option>
                ))}
              </select>

              <div className="tp-modal-actions">
                <DefaultButton
                  title="Cancel"
                  type="back"
                  onClick={() => setIsModalOpen(false)}
                ></DefaultButton>
                <DefaultButton title="Add exercise" type="submit" />
              </div>
            </form>
          </div>
        </div>
      )}

      {deleteConfirmOpen && exerciseToDelete && (
        <div className="tp-modal-overlay">
          <div className="tp-modal tp-modal--small">
            <div className="tp-modal-header">
              <h3>Remove exercise</h3>
              <button type="button" onClick={cancelDelete}>
                ✕
              </button>
            </div>

            <p className="tp-modal-text">
              Are you sure you want to remove{" "}
              <span className="tp-modal-text-strong">
                {exerciseToDelete.name || "this exercise"}
              </span>{" "}
              from this plan?
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

export default TrainingPlanExerciseListPage;
