import { useNavigate, Navigate } from "react-router-dom";
import LoginForm from "../../components/auth/LoginForm";
import UserStore from "../../stores/UserStore";
import logo from "../../assets/images/logo_transparent_bg.png";
import "../../styles/Auth/LoginPage.css";

function LoginPage() {
  const navigate = useNavigate();
  const user = UserStore.getCurrentUser();

  if (user) {
    return <Navigate to="/user" replace />;
  }

  const handleLogin = async (email, password) => {
    const user = await UserStore.login(email, password);
    if (user) {
      navigate("/user", { replace: true });
    }
  };

  return (
    <div className="login-page-root">
      <div className="login-page-content">
        <img src={logo} alt="ROFit logo" className="login-page-logo" />

        <h1 className="login-page-title-main">WELCOME BACK,</h1>
        <h1 className="login-page-title-sub">CONTINUE YOUR JOURNEY, LOG IN.</h1>

        <LoginForm onLogin={handleLogin} />
      </div>
    </div>
  );
}

export default LoginPage;
