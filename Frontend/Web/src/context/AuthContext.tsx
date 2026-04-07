import { createContext, useContext, useState, useEffect } from "react";
import type { ReactNode } from "react";
import type { LoginResponseDto } from "../models/User";
import client from "../services/client";

interface AuthContextType {
  user:    LoginResponseDto | null;
  token:   string | null;
  setUser: (user: LoginResponseDto | null) => void;
  logout:  () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);
const STORAGE_KEY = "alti_session";

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUserState] = useState<LoginResponseDto | null>(() => {
    try {
      const stored = sessionStorage.getItem(STORAGE_KEY);
      if (!stored) return null;
      const parsed: LoginResponseDto = JSON.parse(stored);
      // Verifica expiración al iniciar
      if (parsed.expiresAt && new Date(parsed.expiresAt) < new Date()) {
        sessionStorage.removeItem(STORAGE_KEY);
        return null;
      }
      return parsed;
    } catch {
      sessionStorage.removeItem(STORAGE_KEY);
      return null;
    }
  });

  const token = user?.token ?? null;

  // Verifica expiración cada 30 segundos
  useEffect(() => {
    if (!user?.expiresAt) return;
    const interval = setInterval(() => {
      if (new Date(user.expiresAt) < new Date()) {
        logout();
      }
    }, 30000);
    return () => clearInterval(interval);
  }, [user]);

  const setUser = (u: LoginResponseDto | null) => {
    setUserState(u);
    if (u) {
      sessionStorage.setItem(STORAGE_KEY, JSON.stringify(u));
    } else {
      // Limpia TODO al hacer logout
      sessionStorage.removeItem(STORAGE_KEY);
    }
  };

  const logout = () => {
    // 1 — limpia el estado
    setUserState(null);
    // 2 — limpia sessionStorage
    sessionStorage.removeItem(STORAGE_KEY);
    // 3 — limpia cualquier header que pueda quedar en axios
    delete client.defaults.headers.common["Authorization"];
  };

  return (
    <AuthContext.Provider value={{ user, token, setUser, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
};