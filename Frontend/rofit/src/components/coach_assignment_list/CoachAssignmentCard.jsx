import { useState } from "react";

import userGif from "../../assets/animated_icons/profile.gif";
import userStatic from "../../assets/images/profile.png";
function CoachAssignmentCard({ assignment, onClick }) {
  const [hovered, setHovered] = useState(false);
  const displayName =
    assignment.user?.fullName || assignment.user?.email || "User";

  return (
    <div
      className="coach-assignment-card glass-card"
      onClick={onClick}
      onMouseEnter={() => setHovered(true)}
      onMouseLeave={() => setHovered(false)}
    >
      <div className="coach-assignment-top">
        <div className="coach-assignment-animated-icon">
          <img src={hovered ? userGif : userStatic} alt="User" />
        </div>

        <div className="coach-assignment-text">
          <h3 className="coach-assignment-name">{displayName}</h3>
          <p className="coach-assignment-sub">Tap to see more</p>
        </div>
      </div>

      <div className="coach-assignment-bottom">
        <span className="coach-assignment-cta">View details →</span>
      </div>
    </div>
  );
}
export default CoachAssignmentCard;
