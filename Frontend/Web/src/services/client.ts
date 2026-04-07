import axios from "axios";

const client = axios.create({
  baseURL: "/api",
  headers: { "Content-Type": "application/json" },
  timeout: 10000,
});

// Request — inyecta token en cada request
client.interceptors.request.use(
  config => {
    const stored = sessionStorage.getItem("alti_session");
    if (stored) {
      try {
        const session = JSON.parse(stored);
        // Verifica que el token no haya expirado antes de enviarlo
        if (session?.token && session?.expiresAt) {
          if (new Date(session.expiresAt) > new Date()) {
            config.headers.Authorization = `Bearer ${session.token}`;
          } else {
            // Token expirado — limpia y redirige
            sessionStorage.removeItem("alti_session");
            window.location.href = "/login";
          }
        }
      } catch {
        sessionStorage.removeItem("alti_session");
      }
    } else {
      // Sin sesión — elimina el header por si quedó
      delete config.headers.Authorization;
    }
    return config;
  },
  error => Promise.reject(error)
);

// Response — maneja errores
client.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      sessionStorage.removeItem("alti_session");
      window.location.href = "/login";
      return Promise.reject(new Error("Sesión expirada. Por favor inicie sesión nuevamente."));
    }

    const message =
      error.response?.data?.message ||
      error.response?.data?.error   ||
      error.message                 ||
      "Error desconocido.";

    return Promise.reject(new Error(message));
  }
);

export default client;