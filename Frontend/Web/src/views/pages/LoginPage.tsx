import { useState } from "react";
import { Link } from "react-router-dom";
import useAuthController from "../../controllers/useAuthController";
import FormField from "../components/FormField";
import { validators } from "../../utils/validators";
import { useSEO } from "../../hooks/useSEO";

export default function LoginPage() {
    useSEO({ title: "Iniciar sesión", description: "Acceda a su cuenta para gestionar sus reservas en ALTI Hotel." });

    const { loading, error, handleLogin } = useAuthController();
    const [form, setForm] = useState({ email: "", password: "" });
    const [touched, setTouched] = useState({ email: false, password: false });

    const errors = {
        email: validators.email(form.email),
        password: form.password ? "" : "La contraseña es requerida",
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) =>
        setForm({ ...form, [e.target.name]: e.target.value });

    const handleBlur = (e: React.FocusEvent<HTMLInputElement>) =>
        setTouched({ ...touched, [e.target.name]: true });

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        setTouched({ email: true, password: true });
        if (errors.email || errors.password) return;
        handleLogin({ email: form.email, password: form.password });
    };

    return (
        <div className="min-h-screen bg-bg-base flex pt-20">

            {/* Left image */}
            <div className="hidden lg:block lg:w-1/2 relative">
                <img src="https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=1200&q=80"
                    alt="ALTI Hotel" className="w-full h-full object-cover" />
                <div className="absolute inset-0 bg-black/40" />
                <div className="absolute bottom-16 left-12 text-white">
                    <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Bienvenido de vuelta</span>
                    <h2 className="font-serif text-4xl mt-2 leading-tight">Su refugio de lujo<br />le espera</h2>
                </div>
            </div>

            {/* Right form */}
            <div className="w-full lg:w-1/2 flex items-center justify-center px-8 py-16">
                <div className="w-full max-w-md">
                    <div className="mb-12">
                        <Link to="/" className="inline-flex flex-col mb-10">
                            <span className="font-serif text-text-main text-3xl tracking-[0.1em]">ALTI</span>
                            <span className="text-primary text-[9px] tracking-[0.4em] uppercase mt-0.5">Hotel & Resort</span>
                        </Link>
                        <h1 className="font-serif text-3xl text-text-main mb-2">Iniciar Sesión</h1>
                        <p className="text-text-sub text-xs tracking-wide">Acceda a su cuenta para gestionar sus reservas</p>
                    </div>

                    <form onSubmit={handleSubmit} className="space-y-2">
                        <FormField label="Correo electrónico" name="email" type="email" value={form.email}
                            onChange={handleChange} onBlur={handleBlur}
                            error={errors.email} touched={touched.email}
                            placeholder="su@email.com" required />
                        <FormField label="Contraseña" name="password" type="password" value={form.password}
                            onChange={handleChange} onBlur={handleBlur}
                            error={errors.password} touched={touched.password}
                            placeholder="••••••••" required />

                        {error && (
                            <div className="border-l-2 border-red-400 pl-4 py-2">
                                <p className="text-red-500 text-xs tracking-wide">{error}</p>
                            </div>
                        )}

                        <div className="pt-4">
                            <button type="submit" disabled={loading} className="btn-primary w-full py-4 text-xs">
                                {loading ? "Verificando..." : "Acceder"}
                            </button>
                            <div className="mt-6 pt-6 border-t border-bg-card text-center space-y-3">
                                <Link
                                    to="/forgot-password"
                                    className="block text-text-sub text-xs tracking-wide hover:text-primary transition-colors font-medium"
                                >
                                    ¿Olvidó su contraseña?
                                </Link>
                                <p className="text-text-sub text-xs tracking-wide">
                                    ¿No tiene cuenta?{" "}
                                    <Link to="/register" className="text-primary hover:underline underline-offset-4 font-semibold">
                                        Regístrese aquí
                                    </Link>
                                </p>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}