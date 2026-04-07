import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useSEO } from "../../hooks/useSEO";
import type { BookingResponseDto } from "../../models/Booking";

export default function BookingConfirmationPage() {
  useSEO({ title: "Reserva confirmada", description: "Su reserva en ALTI Hotel ha sido registrada." });

  const location = useLocation();
  const navigate = useNavigate();
  const [visible, setVisible] = useState(false);

  const state = location.state as {
    booking: BookingResponseDto;
    total:   number;
  } | null;

  useEffect(() => {
    if (!state?.booking) { navigate("/my-bookings"); return; }
    // Trigger animation
    setTimeout(() => setVisible(true), 100);
  }, []);

  if (!state?.booking) return null;

  const { booking, total } = state;

  const steps = [
    { icon: "✓", label: "Reserva creada",             done: true  },
    { icon: "✓", label: "Pago registrado",             done: true  },
    { icon: "◌", label: "Confirmación del hotel",      done: false },
    { icon: "◌", label: "Check-in",                    done: false },
  ];

  return (
    <div className="min-h-screen bg-bg-base flex items-center justify-center px-6 py-20">
      <div className="w-full max-w-2xl">

        {/* Hero check */}
        <div
          className="text-center mb-12"
          style={{
            opacity:   visible ? 1 : 0,
            transform: visible ? "translateY(0)" : "translateY(30px)",
            transition:"all 0.8s cubic-bezier(0.16,1,0.3,1)",
          }}
        >
          {/* Animated ring */}
          <div className="relative w-28 h-28 mx-auto mb-8">
            <div
              className="absolute inset-0 rounded-full border-2 border-primary/20"
              style={{ animation: "ripple 2s ease-out infinite" }}
            />
            <div
              className="absolute inset-2 rounded-full border-2 border-primary/30"
              style={{ animation: "ripple 2s 0.4s ease-out infinite" }}
            />
            <div className="absolute inset-0 rounded-full bg-primary/10 border-2 border-primary flex items-center justify-center">
              <span
                className="text-primary text-4xl font-medium font-serif"
                style={{
                  opacity:   visible ? 1 : 0,
                  transform: visible ? "scale(1)" : "scale(0.5)",
                  transition:"all 0.5s 0.4s cubic-bezier(0.34,1.56,0.64,1)",
                }}
              >
                ✓
              </span>
            </div>
          </div>

          <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold block mb-3">
            ¡Excelente elección!
          </span>
          <h1 className="font-serif text-4xl md:text-5xl text-text-main font-medium mb-4">
            Reserva registrada
          </h1>
          <p className="text-text-sub text-sm max-w-md mx-auto leading-relaxed">
            Su reserva ha sido creada exitosamente. Recibirá una confirmación
            por correo electrónico una vez que el hotel apruebe el pago.
          </p>
        </div>

        {/* Booking code highlight */}
        <div
          className="bg-primary text-white text-center py-6 mb-8"
          style={{
            opacity:   visible ? 1 : 0,
            transform: visible ? "translateY(0)" : "translateY(20px)",
            transition:"all 0.8s 0.2s cubic-bezier(0.16,1,0.3,1)",
          }}
        >
          <p className="text-white/60 text-[10px] tracking-[0.4em] uppercase font-semibold mb-2">
            Código de reserva
          </p>
          <p className="font-serif text-4xl font-medium tracking-widest">
            {booking.code}
          </p>
          <p className="text-white/60 text-xs mt-2 font-medium">
            Guarde este código para hacer seguimiento de su reserva
          </p>
        </div>

        {/* Details + status */}
        <div
          className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8"
          style={{
            opacity:   visible ? 1 : 0,
            transform: visible ? "translateY(0)" : "translateY(20px)",
            transition:"all 0.8s 0.35s cubic-bezier(0.16,1,0.3,1)",
          }}
        >
          {/* Booking details */}
          <div className="bg-bg-white border border-bg-card p-6">
            <h2 className="font-serif text-xl text-text-main font-medium mb-5">
              Detalles de la reserva
            </h2>
            <div className="space-y-4">
              {[
                { label:"Check-In",         value: booking.checkInDate  },
                { label:"Check-Out",        value: booking.checkOutDate },
                { label:"Noches",           value: `${booking.nights}`  },
                { label:"Total pagado",     value: `$${total}`, highlight: true },
              ].map(row => (
                <div key={row.label} className="flex justify-between pb-3 border-b border-bg-card last:border-0 last:pb-0">
                  <span className="text-[10px] tracking-[0.2em] uppercase text-text-sub font-semibold">
                    {row.label}
                  </span>
                  <span className={row.highlight
                    ? "font-serif text-xl text-primary font-medium"
                    : "text-sm text-text-main font-medium"
                  }>
                    {row.value}
                  </span>
                </div>
              ))}
            </div>
          </div>

          {/* Progress steps */}
          <div className="bg-bg-white border border-bg-card p-6">
            <h2 className="font-serif text-xl text-text-main font-medium mb-5">
              Estado de su reserva
            </h2>
            <div className="space-y-4">
              {steps.map((step, i) => (
                <div key={i} className="flex items-center gap-4">
                  {/* Step indicator */}
                  <div className={`w-8 h-8 rounded-full flex items-center justify-center flex-shrink-0 border ${
                    step.done
                      ? "bg-primary border-primary"
                      : "bg-bg-card border-bg-card"
                  }`}>
                    <span className={`text-xs font-bold ${step.done ? "text-white" : "text-text-sub"}`}>
                      {step.icon}
                    </span>
                  </div>

                  {/* Label */}
                  <div className="flex-1">
                    <p className={`text-sm font-medium ${step.done ? "text-text-main" : "text-text-sub"}`}>
                      {step.label}
                    </p>
                  </div>

                  {/* Badge */}
                  {step.done ? (
                    <span className="text-[9px] tracking-widest uppercase bg-primary/10 text-primary px-2 py-1 font-semibold">
                      Listo
                    </span>
                  ) : (
                    <span className="text-[9px] tracking-widest uppercase bg-bg-card text-text-sub px-2 py-1 font-semibold">
                      Pendiente
                    </span>
                  )}
                </div>
              ))}
            </div>

            {/* Note */}
            <div className="mt-6 pt-4 border-t border-bg-card">
              <p className="text-text-sub text-xs leading-relaxed font-medium">
                ◈ El hotel confirmará su reserva en un plazo de 24 horas.
              </p>
            </div>
          </div>
        </div>

        {/* Actions */}
        <div
          className="flex flex-col sm:flex-row gap-4"
          style={{
            opacity:   visible ? 1 : 0,
            transform: visible ? "translateY(0)" : "translateY(20px)",
            transition:"all 0.8s 0.5s cubic-bezier(0.16,1,0.3,1)",
          }}
        >
          <Link
            to={`/bookings/${booking.id}`}
            className="btn-primary flex-1 py-4 text-xs text-center"
          >
            Ver detalle de reserva
          </Link>
          <Link
            to="/my-bookings"
            className="btn-outline flex-1 py-4 text-xs text-center"
          >
            Mis reservas
          </Link>
          <Link
            to="/"
            className="btn-outline flex-1 py-4 text-xs text-center"
          >
            Volver al inicio
          </Link>
        </div>
      </div>

      <style>{`
        @keyframes ripple {
          0%   { transform: scale(0.8); opacity: 0.6; }
          100% { transform: scale(1.6); opacity: 0;   }
        }
      `}</style>
    </div>
  );
}