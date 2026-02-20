import AuthService from "../api/services/AuthService";

class UserStore {
  constructor() {
    this.token = localStorage.getItem("token") || null;
    const storedUser = localStorage.getItem("user");
    this.user = storedUser ? JSON.parse(storedUser) : null;
  }

  async login(email, password) {
    const response = await AuthService.login({ email, password });

    const {
      id,
      email: userEmail,
      fullName,
      hasPin,
      token,
      role,
    } = response.data;

    const user = { id, email: userEmail, fullName, hasPin, role };

    this.token = token;
    this.user = user;

    localStorage.setItem("token", token);
    localStorage.setItem("user", JSON.stringify(user));
    localStorage.setItem("role", role);

    return user;
  }

  logout() {
    this.token = null;
    this.user = null;
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    localStorage.removeItem("role");
  }

  isLoggedIn() {
    return !!this.token;
  }

  getCurrentUser() {
    return this.user;
  }
}

export default new UserStore();
