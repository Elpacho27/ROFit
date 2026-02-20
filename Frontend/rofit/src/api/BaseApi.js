import axios from "axios";

const BASE_URL = "http://localhost:5293/api";
const axiosInstance = axios.create({
  baseURL: BASE_URL,
});

axiosInstance.interceptors.request.use((config) => {
  try {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
  } catch (error) {
    console.error("Error retrieving token from localStorage", error);
  }
  return config;
});

const baseApi = {
  getAll: async (URL, params) => {
    try {
      const queryString = new URLSearchParams(params).toString();
      const fullUrl = `${BASE_URL}${URL}?${queryString}`;
      const response = await axios.get(fullUrl, {});
      return response;
    } catch (error) {
      console.error("Error in getAll:", error);
      throw error;
    }
  },
  get: (url, config) => axiosInstance.get(url, config),
  post: (url, data) => axiosInstance.post(url, data),
  put: (url, data) => axiosInstance.put(url, data),
  delete: (url) => axiosInstance.delete(url),
};

export default baseApi;
