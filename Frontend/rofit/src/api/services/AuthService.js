import baseApi from "../BaseApi";
const AuthService = {
  register: (data) => baseApi.post("/Auth/register", data),
  login: (data) => baseApi.post("/Auth/login", data),
};

export default AuthService;
