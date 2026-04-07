import { useState } from "react";
import { Link } from "react-router-dom";
import useAuthController from "../../controllers/useAuthController";
import FormField from "../components/FormField";
import { validators } from "../../utils/validators";
import { useSEO } from "../../hooks/useSEO";

type FormKey = "firstName"|"lastName"|"email"|"password"|"phone";

export default function RegisterPage() {
  useSEO({ title: "Crear cuenta", description: "Únase a ALTI Hotel y disfrute de beneficios exclusivos." });

  const { loading, error, handleRegister } = useAuthController();
  const [form, setForm]       = useState<Record<FormKey,string>>({ firstName:"",lastName:"",email:"",password:"",phone:"" });
  const [touched, setTouched] = useState<Record<FormKey,boolean>>({ firstName:false,lastName:false,email:false,password:false,phone:false });

  const errors: Record<FormKey,string> = {
    firstName: validators.firstName(form.firstName),
    lastName:  validators.lastName(form.lastName),
    email:     validators.email(form.email),
    password:  validators.password(form.password),
    phone:     validators.phone(form.phone),
  };

  const isValid = !Object.values(errors).some(e => e !== "");

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) =>
    setForm({ ...form, [e.target.name]: e.target.value });
  const handleBlur = (e: React.FocusEvent<HTMLInputElement>) =>
    setTouched({ ...touched, [e.target.name]: true });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setTouched({ firstName:true,lastName:true,email:true,password:true,phone:true });
    if (!isValid) return;
    handleRegister({ firstName:form.firstName, lastName:form.lastName, email:form.email, password:form.password, phone:form.phone||undefined });
  };

  return (
    <div className="min-h-screen bg-bg-base flex pt-20">

      {/* Left form */}
      <div className="w-full lg:w-1/2 flex items-center justify-center px-8 py-16">
        <div className="w-full max-w-md">
          <div className="mb-12">
            <Link to="/" className="inline-flex flex-col mb-10">
              <span className="font-serif text-text-main text-3xl tracking-[0.1em]">ALTI</span>
              <span className="text-primary text-[9px] tracking-[0.4em] uppercase mt-0.5">Hotel & Resort</span>
            </Link>
            <h1 className="font-serif text-3xl text-text-main mb-2">Crear Cuenta</h1>
            <p className="text-text-sub text-xs tracking-wide">Únase a la experiencia ALTI Hotel</p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-1">
            <div className="grid grid-cols-2 gap-4">
              <FormField label="Nombre"   name="firstName" value={form.firstName} onChange={handleChange} onBlur={handleBlur} error={errors.firstName} touched={touched.firstName} placeholder="Juan"  required />
              <FormField label="Apellido" name="lastName"  value={form.lastName}  onChange={handleChange} onBlur={handleBlur} error={errors.lastName}  touched={touched.lastName}  placeholder="Pérez" required />
            </div>
            <FormField label="Correo electrónico"  name="email"    type="email"    value={form.email}    onChange={handleChange} onBlur={handleBlur} error={errors.email}    touched={touched.email}    placeholder="su@email.com"         required />
            <FormField label="Contraseña"          name="password" type="password" value={form.password} onChange={handleChange} onBlur={handleBlur} error={errors.password} touched={touched.password} placeholder="Mínimo 8 caracteres"   required />
            <FormField label="Teléfono (opcional)" name="phone"    type="tel"      value={form.phone}    onChange={handleChange} onBlur={handleBlur} error={errors.phone}    touched={touched.phone}    placeholder="+1 809 000 0000" />

            {error && (
              <div className="border-l-2 border-red-400 pl-4 py-2">
                <p className="text-red-500 text-xs tracking-wide">{error}</p>
              </div>
            )}

            <div className="pt-4">
              <button type="submit" disabled={loading} className="btn-primary w-full py-4 text-xs">
                {loading ? "Creando cuenta..." : "Crear Cuenta"}
              </button>
            </div>
          </form>

          <div className="mt-10 pt-8 border-t border-bg-card">
            <p className="text-text-sub text-xs tracking-wide text-center">
              ¿Ya tiene cuenta?{" "}
              <Link to="/login" className="text-primary hover:underline underline-offset-4">Iniciar sesión</Link>
            </p>
          </div>
        </div>
      </div>

      {/* Right image */}
      <div className="hidden lg:block lg:w-1/2 relative">
        <img src="https://images.unsplash.com/photo-1618773928121-c32242e63f39?w=1200&q=80"
          alt="ALTI Hotel" className="w-full h-full object-cover" />
        <div className="absolute inset-0 bg-black/40" />
        <div className="absolute bottom-16 right-12 text-right text-white">
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Únase a ALTI</span>
          <h2 className="font-serif text-4xl mt-2 leading-tight">Una experiencia<br />diseñada para usted</h2>
        </div>
      </div>
    </div>
  );
}