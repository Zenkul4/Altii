import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useToast } from "../context/ToastContext";
import AuthService from "../services/AuthService";
import type { LoginDto, CreateUserDto } from "../models/User";
import { useNotifications } from "../context/NotificationContext";

const useAuthController = () => {
    const { setUser, logout } = useAuth();
    const { showToast } = useToast();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
     const { addNotification } = useNotifications();

    const handleLogin = async (dto: LoginDto) => {
        setError("");
        setLoading(true);
        try {
            const user = await AuthService.login(dto);
            setUser(user);
            showToast(`Bienvenido, ${user.fullName.split(" ")[0]}`, "success");
            addNotification("system", "Sesión iniciada", `Bienvenido de vuelta, ${user.fullName.split(" ")[0]}`);

            navigate("/");
        } catch (err: any) {
            setError(err.message ?? "Credenciales incorrectas.");
        } finally {
            setLoading(false);
        }
    };

    const handleRegister = async (dto: CreateUserDto) => {
        setError("");
        setLoading(true);
        try {
            const user = await AuthService.register(dto);
            setUser(user);
            showToast("Cuenta creada exitosamente", "success");
            navigate("/");
        } catch (err: any) {
            setError(err.message ?? "Error al registrarse.");
        } finally {
            setLoading(false);
        }
    };

    const handleLogout = () => {
        logout();
        showToast("Sesión cerrada", "info");
        navigate("/login");
    };

    return { loading, error, handleLogin, handleRegister, handleLogout };
};

export default useAuthController;