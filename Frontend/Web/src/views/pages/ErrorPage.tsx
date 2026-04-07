import { Link, useRouteError, isRouteErrorResponse } from "react-router-dom";
import { useSEO } from "../../hooks/useSEO";

interface ErrorPageProps {
  code?:    number;
  message?: string;
}

export default function ErrorPage({ code, message }: ErrorPageProps) {
  useSEO({ title: "Error", description: "Ha ocurrido un error en ALTI Hotel." });

  // Si se usa como errorElement de React Router
  const routeError = useRouteError?.();
  const statusCode = code
    ?? (isRouteErrorResponse(routeError) ? routeError.status : 500);
  const errorMessage = message
    ?? (isRouteErrorResponse(routeError) ? routeError.statusText : "Ha ocurrido un error inesperado.");

  const info: Record<number, { title: string; desc: string; icon: string }> = {
    400: { icon:"◌", title:"Solicitud incorrecta",  desc:"Los datos enviados no son válidos. Verifique la información e intente nuevamente."         },
    401: { icon:"⊘", title:"No autorizado",          desc:"Necesita iniciar sesión para acceder a esta sección."                                      },
    403: { icon:"⊗", title:"Acceso denegado",        desc:"No tiene permisos para ver este contenido."                                               },
    404: { icon:"◎", title:"Página no encontrada",   desc:"La página que busca no existe o ha sido movida."                                          },
    500: { icon:"◈", title:"Error del servidor",     desc:"Algo salió mal en nuestro servidor. Estamos trabajando para solucionarlo."                 },
    503: { icon:"◉", title:"Servicio no disponible", desc:"El servicio está temporalmente fuera de línea. Por favor intente en unos minutos."         },
  };

  const { icon, title, desc } = info[statusCode] ?? info[500];

  return (
    <div className="min-h-screen bg-bg-base flex items-center justify-center px-6">
      <div className="max-w-2xl w-full text-center">

        {/* Big error code */}
        <div className="relative mb-8">
          <p className="font-serif text-[180px] md:text-[220px] text-text-sub/5 leading-none select-none font-medium">
            {statusCode}
          </p>

          {/* Icon overlay */}
          <div className="absolute inset-0 flex items-center justify-center">
            <div
              className="w-24 h-24 rounded-full bg-primary/10 border-2 border-primary/30 flex items-center justify-center"
              style={{ animation: "pulse-error 3s ease-in-out infinite" }}
            >
              <span className="text-primary text-4xl">{icon}</span>
            </div>
          </div>
        </div>

        {/* Info */}
        <div className="mb-10">
          <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold block mb-4">
            Error {statusCode}
          </span>
          <h1 className="font-serif text-3xl md:text-5xl text-text-main font-medium mb-4">
            {title}
          </h1>
          <p className="text-text-sub text-sm leading-relaxed max-w-md mx-auto font-medium">
            {errorMessage !== title ? errorMessage : desc}
          </p>
        </div>

        {/* What to do */}
        <div className="bg-bg-card p-6 mb-10 text-left max-w-md mx-auto">
          <p className="text-[10px] tracking-[0.3em] uppercase text-primary font-semibold mb-4">
            ¿Qué puede hacer?
          </p>
          <div className="space-y-3">
            {[
              "Verifique que la URL sea correcta",
              "Regrese a la página anterior",
              "Vaya al inicio y comience de nuevo",
              "Si el problema persiste, contáctenos",
            ].map((tip, i) => (
              <div key={i} className="flex items-center gap-3">
                <span className="text-primary text-xs flex-shrink-0">◈</span>
                <span className="text-text-sub text-sm font-medium">{tip}</span>
              </div>
            ))}
          </div>
        </div>

        {/* Actions */}
        <div className="flex flex-col sm:flex-row gap-4 justify-center">
          <button
            onClick={() => window.history.back()}
            className="btn-outline px-10 py-4 text-xs"
          >
            ← Volver atrás
          </button>
          <Link to="/" className="btn-primary px-10 py-4 text-xs">
            Ir al inicio
          </Link>
          <Link to="/contact" className="btn-outline px-10 py-4 text-xs">
            Contactar soporte
          </Link>
        </div>
      </div>

      <style>{`
        @keyframes pulse-error {
          0%, 100% { transform: scale(1);    box-shadow: 0 0 0 0 rgba(139,95,191,0); }
          50%       { transform: scale(1.05); box-shadow: 0 0 30px 6px rgba(139,95,191,0.15); }
        }
      `}</style>
    </div>
  );
}