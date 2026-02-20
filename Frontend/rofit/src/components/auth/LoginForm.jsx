import { useState } from "react";
import { TextInput } from "../common/TextInput";

function LoginForm({ onLogin }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      await onLogin(email, password);
    } catch (err) {
      setError("Login failed. Check your credentials.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="login-form">
      <div className="login-form-field">
        <TextInput
          id="email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
          placeholder="EMAIL"
        />
      </div>

      <div className="login-form-field">
        <TextInput
          id="password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          placeholder="PASSWORD"
        />
      </div>

      {error && <p className="login-form-error">{error}</p>}

      <button type="submit" disabled={loading} className="login-form-button">
        {loading ? "Logging in..." : "LOGIN"}
      </button>
    </form>
  );
}

export default LoginForm;
