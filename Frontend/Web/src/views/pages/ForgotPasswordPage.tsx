import { useState } from "react";
import { Link } from "react-router-dom";
import { useSEO } from "../../hooks/useSEO";


type Step = "email" | "sent";

export default function ForgotPasswordPage() {
  useSEO({ title: "Recuperar contraseña", description: "Recupere el acceso a su cuenta ALTI Hotel." });

  const [step,    setStep]    = useState<Step>("email");
  const [email,   setEmail]   = useState("");
  const [loading, setLoading] = useState(false);
  const [error,   setError]   = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!email.trim()) { setError("Ingrese su correo electrónico."); return; }
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) { setError("El correo no es válido."); return; }

    setError("");
    setLoading(true);

    try {
      // Endpoint futuro — por ahora simula el envío
      // await client.post("/Auth/forgot-password", { email });
      await new Promise(r => setTimeout(r, 1200)); // simula delay
      setStep("sent");
    } catch (err: any) {
      setError(err.message ?? "Error al procesar la solicitud.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-bg-base flex pt-20">

      {/* Left image */}
      <div className="hidden lg:block lg:w-1/2 relative">
        <img
          src="https://images.unsplash.com/photo-1618773928121-c32242e63f39?w=1200&q=80"
          alt="ALTI Hotel"
          className="w-full h-full object-cover"
        />
        <div className="absolute inset-0 bg-black/50" />
        <div className="absolute bottom-16 left-12 text-white">
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase font-semibold">
            Recuperar acceso
          </span>
          <h2 className="font-serif text-4xl mt-2 font-medium leading-tight">
            Le ayudamos a<br />recuperar su cuenta
          </h2>
        </div>
      </div>

      {/* Right form */}
      <div className="w-full lg:w-1/2 flex items-center justify-center px-8 py-16">
        <div className="w-full max-w-md">

          <div className="mb-12">
            <Link to="/" className="inline-flex flex-col mb-10">
              <span className="font-serif text-text-main text-3xl tracking-[0.1em] font-medium">ALTI</span>
              <span className="text-primary text-[9px] tracking-[0.4em] uppercase mt-0.5 font-semibold">Hotel & Resort</span>
            </Link>
            <h1 className="font-serif text-3xl text-text-main mb-2 font-medium">
              Recuperar contraseña
            </h1>
            <p className="text-text-sub text-xs tracking-wide">
              {step === "email"
                ? "Ingrese su correo y le enviaremos instrucciones"
                : "Revise su bandeja de entrada"
              }
            </p>
          </div>

          {step === "email" ? (
            <form onSubmit={handleSubmit} className="space-y-8">

              {/* Email field */}
              <div>
                <label className="block text-[10px] tracking-[0.2em] uppercase text-text-sub mb-3 font-semibold">
                  Correo electrónico
                </label>
                <input
                  type="email"
                  value={email}
                  onChange={e => setEmail(e.target.value)}
                  placeholder="su@email.com"
                  required
                  className="input-field"
                  autoFocus
                />
              </div>

              {error && (
                <div className="border-l-2 border-red-400 pl-4 py-2">
                  <p className="text-red-500 text-xs tracking-wide font-medium">{error}</p>
                </div>
              )}

              <button
                type="submit"
                disabled={loading}
                className="btn-primary w-full py-4 text-xs relative overflow-hidden group"
              >
                <span className="relative z-10">
                  {loading ? "Enviando..." : "Enviar instrucciones"}
                </span>
                <div className="absolute inset-0 -translate-x-full group-hover:translate-x-full transition-transform duration-700 bg-gradient-to-r from-transparent via-white/20 to-transparent" />
              </button>

              <div className="pt-4 border-t border-bg-card text-center">
                <p className="text-text-sub text-xs tracking-wide">
                  ¿Recuerda su contraseña?{" "}
                  <Link to="/login" className="text-primary hover:underline underline-offset-4 font-semibold">
                    Iniciar sesión
                  </Link>
                </p>
              </div>
            </form>

          ) : (
            /* Success state */
            <div className="space-y-8">
              <div className="text-center py-8">
                {/* Animated check */}
                <div
                  className="w-20 h-20 rounded-full bg-primary/10 border-2 border-primary/30 flex items-center justify-center mx-auto mb-6"
                  style={{ animation: "pulse-badge 2s ease-in-out infinite" }}
                >
                  <span className="text-primary text-3xl">✓</span>
                </div>

                <h2 className="font-serif text-2xl text-text-main font-medium mb-3">
                  Correo enviado
                </h2>
                <p className="text-text-sub text-sm leading-relaxed max-w-xs mx-auto">
                  Hemos enviado instrucciones de recuperación a
                </p>
                <p className="text-primary font-semibold text-sm mt-2 mb-6">
                  {email}
                </p>
                <p className="text-text-sub text-xs leading-relaxed max-w-xs mx-auto">
                  Revise también su carpeta de spam. El enlace expira en 30 minutos.
                </p>
              </div>

              {/* Info steps */}
              <div className="bg-bg-card p-6 space-y-4">
                {[
                  { num: "1", text: "Abra el correo enviado por ALTI Hotel"      },
                  { num: "2", text: "Haga click en el enlace de recuperación"     },
                  { num: "3", text: "Cree una nueva contraseña segura"            },
                  { num: "4", text: "Inicie sesión con su nueva contraseña"       },
                ].map(step => (
                  <div key={step.num} className="flex items-center gap-4">
                    <div className="w-7 h-7 rounded-full bg-primary/20 border border-primary/30 flex items-center justify-center flex-shrink-0">
                      <span className="text-primary text-xs font-bold">{step.num}</span>
                    </div>
                    <p className="text-text-sub text-sm font-medium">{step.text}</p>
                  </div>
                ))}
              </div>

              <div className="flex flex-col gap-3">
                <button
                  onClick={() => { setStep("email"); setEmail(""); }}
                  className="btn-outline w-full py-3.5 text-xs"
                >
                  Usar otro correo
                </button>
                <Link to="/login" className="btn-primary w-full py-3.5 text-xs text-center">
                  Volver al inicio de sesión
                </Link>
              </div>
            </div>
          )}
        </div>
      </div>

      <style>{`
        @keyframes pulse-badge {
          0%, 100% { transform: scale(1);    box-shadow: 0 0 0 0 rgba(139,95,191,0); }
          50%       { transform: scale(1.05); box-shadow: 0 0 20px 4px rgba(139,95,191,0.2); }
        }
      `}</style>
    </div>
  );
}