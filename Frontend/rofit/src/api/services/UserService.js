import { use } from "react";
import baseApi from "../BaseApi";

const UserService = {
  getById: (id) => baseApi.get(`/User/${id}`),
};

export default UserService;
