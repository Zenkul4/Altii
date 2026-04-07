import { Navigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

export default function ProtectedRoute({ children }: { children: React.ReactElement }) {
  const { user } = useAuth();

  if (!user) return <Navigate to="/login" replace />;

  if (user.expiresAt && new Date(user.expiresAt) < new Date()) {
    return <Navigate to="/login" replace />;
  }

  return children;
}