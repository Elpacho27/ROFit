import { useNavigate } from "react-router-dom";
import UserStore from "../../stores/UserStore";
import "../../styles/User/UserPage.css";

function UserPage() {
  const navigate = useNavigate();
  const user = UserStore.getCurrentUser();

  if (!user) {
    navigate("/login");
    return null;
  }

  const handleLogout = () => {
    UserStore.logout();
    navigate("/login");
  };

  return (
    <div className="user-page">
      <div className="user-card">
        <div className="user-avatar">
          {(user.fullName || user.email || "?").charAt(0).toUpperCase()}
        </div>

        <h1 className="user-name">{user.fullName || user.email}</h1>
        <p className="user-role">{user.role}</p>

        <div className="user-info-grid">
          <div className="user-info-item">
            <span className="user-info-label">Email</span>
            <span className="user-info-value">{user.email}</span>
          </div>
          <div className="user-info-item">
            <span className="user-info-label">Account Status</span>
            <span className="user-info-value">
              <span className="user-status-badge">Active</span>
            </span>
          </div>
          <div className="user-info-item">
            <span className="user-info-label">Security</span>
            <span className="user-info-value">
              {user.hasPin ? (
                <span className="user-security-badge user-security-badge--active">
                  ✓ PIN Enabled
                </span>
              ) : (
                <span className="user-security-badge user-security-badge--inactive">
                  ○ No PIN
                </span>
              )}
            </span>
          </div>
        </div>

        <div className="user-actions">
          <button className="user-button user-button--secondary">
            Edit Profile
          </button>
          <button
            className="user-button user-button--danger"
            onClick={handleLogout}
          >
            Logout
          </button>
        </div>
      </div>
    </div>
  );
}

export default UserPage;
