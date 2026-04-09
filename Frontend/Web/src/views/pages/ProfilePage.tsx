import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { useToast } from "../../context/ToastContext";
import UserService from "../../services/UserService";
import FormField from "../components/FormField";
import { validators } from "../../utils/validators";
import { useSEO } from "../../hooks/useSEO";

const roleLabel: Record<number,string> = { 0:"Huésped", 1:"Recepcionista", 2:"Administrador" };

export default function ProfilePage() {
  useSEO({ title: "Mi perfil", description: "Gestione su información personal en ALTI Hotel." });

  const { user, setUser, logout } = useAuth();
  const { showToast }             = useToast();
  const navigate                  = useNavigate();

  const [form, setForm] = useState({
    firstName: user?.fullName.split(" ")[0] ?? "",
    lastName:  user?.fullName.split(" ").slice(1).join(" ") ?? "",
    phone:     "",
  });
  const [touched, setTouched] = useState({ firstName:false, lastName:false, phone:false });
  const errors = {
    firstName: validators.firstName(form.firstName),
    lastName:  validators.lastName(form.lastName),
    phone:     validators.phone(form.phone),
  };
  const [loading, setLoading] = useState(false);

  if (!user) { navigate("/login"); return null; }

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm({ ...form, [e.target.name]: e.target.value });
  const handleBlur = (e: React.FocusEvent<HTMLInputElement>) =>
    setTouched({ ...touched, [e.target.name]: true });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setTouched({ firstName:true, lastName:true, phone:true });
    if (errors.firstName||errors.lastName) return;
    setLoading(true);
    try {
      const updated = await UserService.update(user.id, { firstName:form.firstName, lastName:form.lastName, phone:form.phone||undefined });
      setUser({ ...user, fullName:`${updated.firstName} ${updated.lastName}` });
      showToast("Perfil actualizado correctamente","success");
} catch (err: unknown) {
    const message = err instanceof Error ? err.message : "Error al actualizar el perfil.";
    showToast(message, "error");
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => { logout(); showToast("Sesión cerrada","info"); navigate("/"); };

  return (
    <div className="min-h-screen bg-bg-base pt-20">
      <div className="bg-bg-card py-14 px-6">
        <div className="max-w-4xl mx-auto">
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Mi cuenta</span>
          <h1 className="font-serif text-4xl md:text-5xl text-text-main mt-2">Perfil</h1>
          <p className="text-text-sub text-xs tracking-widest mt-2">{user.email}</p>
        </div>
      </div>

      <div className="max-w-4xl mx-auto px-6 py-12">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">

          {/* Sidebar */}
          <div className="space-y-4">
            <div className="bg-bg-white border border-bg-card p-6 text-center">
              <div className="w-16 h-16 rounded-full bg-primary/20 flex items-center justify-center mx-auto mb-4">
                <span className="font-serif text-primary text-2xl">{user.fullName.charAt(0)}</span>
              </div>
              <h3 className="font-serif text-xl text-text-main">{user.fullName}</h3>
              <p className="text-[10px] tracking-[0.2em] uppercase text-text-sub mt-1">{roleLabel[user.role]}</p>
            </div>

            <div className="bg-bg-white border border-bg-card p-6 space-y-4">
              {[{to:"/my-bookings",label:"Mis reservas"},{to:"/rooms",label:"Ver habitaciones"}].map(link => (
                <Link key={link.to} to={link.to}
                  className="flex items-center justify-between text-text-main hover:text-primary transition-colors">
                  <span className="text-[10px] tracking-[0.2em] uppercase">{link.label}</span>
                  <span>→</span>
                </Link>
              ))}
              <button onClick={handleLogout}
                className="w-full text-left flex items-center justify-between text-red-400 hover:text-red-600 transition-colors">
                <span className="text-[10px] tracking-[0.2em] uppercase">Cerrar sesión</span>
                <span>→</span>
              </button>
            </div>
          </div>

          {/* Form */}
          <div className="lg:col-span-2">
            <div className="bg-bg-white border border-bg-card p-8">
              <h2 className="font-serif text-2xl text-text-main mb-2">Información personal</h2>
              <p className="text-text-sub text-xs tracking-wide mb-8">Actualice sus datos de contacto</p>

              <form onSubmit={handleSubmit} className="space-y-2">
                <div className="grid grid-cols-2 gap-4">
                  <FormField label="Nombre"   name="firstName" value={form.firstName} onChange={handleChange} onBlur={handleBlur} error={errors.firstName} touched={touched.firstName} required />
                  <FormField label="Apellido" name="lastName"  value={form.lastName}  onChange={handleChange} onBlur={handleBlur} error={errors.lastName}  touched={touched.lastName}  required />
                </div>

                <div>
                  <label className="block text-[10px] tracking-[0.2em] uppercase text-text-sub mb-3">Correo electrónico</label>
                  <input type="email" value={user.email} disabled className="input-field opacity-40 cursor-not-allowed" />
                  <p className="text-[10px] text-text-sub mt-1">El email no puede modificarse</p>
                </div>

                <FormField label="Teléfono (opcional)" name="phone" type="tel" value={form.phone}
                  onChange={handleChange} onBlur={handleBlur}
                  error={errors.phone} touched={touched.phone} placeholder="+1 809 000 0000" />

                <div className="pt-4">
                  <button type="submit" disabled={loading} className="btn-primary py-4 px-12">
                    {loading?"Guardando...":"Guardar cambios"}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}