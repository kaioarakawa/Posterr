import axios from "axios";

const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL,
});

api.interceptors.request.use(
  (config) => {
    // Add headers, authentication tokens, etc. if needed
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default api;
