import { useEffect, useState, useCallback } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import BookingService from "../../services/BookingService";
import PaymentService from "../../services/PaymentService";
import { useToast } from "../../context/ToastContext";
import type { BookingResponseDto } from "../../models/Booking";
import type { PaymentResponseDto } from "../../models/Payment";
import { BookingStatusLabel, BookingStatusColor } from "../../models/Booking";
import { PaymentStatusLabel } from "../../models/Payment";

function useCountdown(expiresAt: string | null | undefined) {
  const [timeLeft, setTimeLeft] = useState("");
  const [expired,  setExpired]  = useState(false);

  useEffect(() => {
    if (!expiresAt) return;
    const tick = () => {
      try {
        const diff = new Date(expiresAt).getTime() - Date.now();
        if (isNaN(diff) || diff <= 0) { setTimeLeft("00:00"); setExpired(true); return; }
        const m = Math.floor(diff / 60000);
        const s = Math.floor((diff % 60000) / 1000);
        setTimeLeft(`${String(m).padStart(2,"0")}:${String(s).padStart(2,"0")}`);
      } catch { setExpired(true); }
    };
    tick();
    const t = setInterval(tick, 1000);
    return () => clearInterval(t);
  }, [expiresAt]);

  return { timeLeft, expired };
}

const statusIcon: Record<number, string> = {
  0: "◌", 1: "✓", 2: "⬤", 3: "◎", 4: "✕", 5: "⊘"
};

export default function BookingDetailPage() {
  const { id }        = useParams<{ id: string }>();
  const navigate      = useNavigate();
  const { showToast } = useToast();

  const [booking,    setBooking]    = useState<BookingResponseDto | null>(null);
  const [payments,   setPayments]   = useState<PaymentResponseDto[]>([]);
  const [loading,    setLoading]    = useState(true);
  const [error,      setError]      = useState("");
  const [cancelling, setCancelling] = useState(false);

  const isPending      = booking?.status === 0;
  const hasCashPayment = payments.some(p => p.paymentMethod === "Cash");

  const { timeLeft, expired } = useCountdown(
    isPending && !hasCashPayment ? booking?.expiresAt : null
  );

  const load = useCallback(async () => {
    if (!id) return;
    setLoading(true);
    setError("");
    try {
      const b = await BookingService.getById(Number(id));
      setBooking(b);
    } catch (err: any) {
      setError(err.message ?? "Error al cargar la reserva.");
      setLoading(false);
      return;
    }
    try {
      const p = await PaymentService.getByBooking(Number(id));
      setPayments(Array.isArray(p) ? p : []);
    } catch {
      setPayments([]);
    }
    setLoading(false);
  }, [id]);

  useEffect(() => { load(); }, [load]);

  const handleCancel = async () => {
    if (!booking || !confirm("¿Está seguro de cancelar esta reserva?")) return;
    setCancelling(true);
    try {
      await BookingService.cancel(booking.id);
      showToast("Reserva cancelada correctamente", "success");
      await load();
    } catch (err: any) {
      showToast(err.message ?? "No se pudo cancelar.", "error");
    } finally {
      setCancelling(false);
    }
  };

  if (loading) return (
    <div className="min-h-screen bg-cream pt-20 flex items-center justify-center">
      <div className="w-8 h-8 border-2 border-[#C9A96E] border-t-transparent rounded-full animate-spin" />
    </div>
  );

  if (error || !booking) return (
    <div className="min-h-screen bg-cream pt-20 flex items-center justify-center text-center px-6">
      <div>
        <p className="font-serif text-3xl text-dark/30 mb-2">
          {error || "Reserva no encontrada"}
        </p>
        <p className="text-muted text-xs tracking-widest uppercase mb-8">
          {error ? "Verifique que la API esté corriendo" : "El ID no existe"}
        </p>
        <Link to="/my-bookings" className="btn-gold">Mis reservas</Link>
      </div>
    </div>
  );

  const nights        = booking.nights        ?? 0;
  const pricePerNight = booking.pricePerNight ?? booking.totalPrice ?? 0;
  const totalPrice    = booking.totalPrice    ?? 0;
  const statusLabel   = BookingStatusLabel[booking.status]  ?? `Estado ${booking.status}`;
  const statusColor   = BookingStatusColor[booking.status]  ?? "bg-gray-100 text-gray-600";
  const icon          = statusIcon[booking.status]          ?? "◌";

  return (
    <div className="min-h-screen bg-cream pt-20">
      <div className="max-w-4xl mx-auto px-6 py-12">

        <button
          onClick={() => navigate("/my-bookings")}
          className="text-[10px] tracking-[0.2em] uppercase text-muted hover:text-[#C9A96E] transition-colors flex items-center gap-2 mb-10"
        >
          ← Mis reservas
        </button>

        {/* Header */}
        <div className="bg-[#0C0C0C] p-8 mb-6">
          <div className="flex flex-col md:flex-row justify-between items-start gap-4">
            <div>
              <span className="text-[#C9A96E] text-[10px] tracking-[0.4em] uppercase">Reserva</span>
              <h1 className="font-serif text-4xl text-white mt-1">{booking.code}</h1>
            </div>
            <div className="text-right">
              <span className="text-[#C9A96E] text-3xl">{icon}</span>
              <p className={`text-[9px] tracking-[0.2em] uppercase px-3 py-1 mt-2 inline-block ${statusColor}`}>
                {statusLabel}
              </p>
            </div>
          </div>
        </div>

        {/* Countdown — solo si no es pago en efectivo */}
        {isPending && !hasCashPayment && !expired && timeLeft && (
          <div className="bg-amber-50 border border-amber-200 p-6 mb-6 flex items-center justify-between">
            <div>
              <p className="text-[10px] tracking-[0.2em] uppercase text-amber-600 mb-1">
                Tiempo para completar el pago
              </p>
              <p className="text-dark text-sm">
                Su reserva expirará si no se completa el pago a tiempo.
              </p>
            </div>
            <div className="text-right">
              <p className="font-serif text-4xl text-amber-600">{timeLeft}</p>
              <p className="text-[10px] tracking-widest uppercase text-amber-400">
                minutos restantes
              </p>
            </div>
          </div>
        )}

        {/* Expirado — solo si no es efectivo */}
        {isPending && !hasCashPayment && expired && (
          <div className="border-l-4 border-red-400 pl-6 py-4 bg-white mb-6">
            <p className="text-red-600 text-sm font-medium">
              Esta reserva ha expirado. Por favor realice una nueva reserva.
            </p>
          </div>
        )}

        {/* Mensaje efectivo pendiente */}
        {isPending && hasCashPayment && (
          <div className="border-l-4 border-[#C9A96E] pl-6 py-4 bg-white mb-6">
            <p className="text-[10px] tracking-[0.2em] uppercase text-[#C9A96E] mb-1">
              Pago en efectivo pendiente
            </p>
            <p className="text-dark text-sm">
              Su pago en efectivo será confirmado por el personal del hotel al momento del check-in.
            </p>
          </div>
        )}

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">

          {/* Detalles */}
          <div className="bg-white border border-[#E8E3DA] p-8">
            <h2 className="font-serif text-2xl text-dark mb-6">Detalles</h2>
            <div className="space-y-4">
              {[
                { label: "Check-In",         value: booking.checkInDate  ?? "—" },
                { label: "Check-Out",        value: booking.checkOutDate ?? "—" },
                { label: "Noches",           value: `${nights}`              },
                { label: "Precio por noche", value: `$${pricePerNight}`      },
                { label: "Total",            value: `$${totalPrice}`, highlight: true },
              ].map(row => (
                <div key={row.label} className="flex justify-between pb-3 border-b border-[#F0EBE3]">
                  <span className="text-[10px] tracking-[0.2em] uppercase text-muted">
                    {row.label}
                  </span>
                  <span className={row.highlight
                    ? "font-serif text-xl text-[#C9A96E]"
                    : "text-sm text-dark font-medium"
                  }>
                    {row.value}
                  </span>
                </div>
              ))}
              {booking.notes && (
                <div className="pt-2">
                  <p className="text-[10px] tracking-[0.2em] uppercase text-muted mb-2">Notas</p>
                  <p className="text-sm text-dark/70 italic">{booking.notes}</p>
                </div>
              )}
            </div>
          </div>

          {/* Pagos */}
          <div className="bg-white border border-[#E8E3DA] p-8">
            <h2 className="font-serif text-2xl text-dark mb-6">Pagos</h2>
            {payments.length === 0 ? (
              <div className="text-center py-8">
                <p className="text-muted text-xs tracking-widest uppercase mb-4">
                  Sin pagos registrados
                </p>
                {booking.status === 0 && !expired && (
                  <button
                    onClick={() => navigate(`/bookings/${booking.id}/pay`, {
                      state: { booking, total: totalPrice }
                    })}
                    className="btn-gold text-xs py-3 px-8"
                  >
                    Realizar pago
                  </button>
                )}
              </div>
            ) : (
              <div className="space-y-4">
                {payments.map(p => (
                  <div key={p.id} className="border border-[#F0EBE3] p-4">
                    <div className="flex justify-between items-start mb-2">
                      <span className="font-serif text-xl text-dark">${p.amount ?? 0}</span>
                      <span className={`text-[9px] tracking-[0.2em] uppercase px-2 py-0.5 ${
                        p.status === 1 ? "bg-green-100 text-green-700"
                        : p.status === 0 ? "bg-yellow-100 text-yellow-700"
                        : "bg-red-100 text-red-700"
                      }`}>
                        {PaymentStatusLabel[p.status] ?? `Estado ${p.status}`}
                      </span>
                    </div>
                    <div className="space-y-1 text-xs text-muted">
                      {p.paymentMethod     && <p>Método: {p.paymentMethod}</p>}
                      {p.externalReference && <p>Ref: {p.externalReference}</p>}
                      {p.processedAt       && (
                        <p>
                          Procesado:{" "}
                          {new Date(p.processedAt).toLocaleDateString("es-DO")}
                        </p>
                      )}
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>

        {/* Actions */}
        <div className="flex gap-4 mt-6">
          <Link to="/my-bookings" className="btn-outline-gold flex-1 text-center py-3.5">
            Volver a mis reservas
          </Link>
          {(booking.status === 0 || booking.status === 1) && (
            <button
              onClick={handleCancel}
              disabled={cancelling}
              className="flex-1 py-3.5 text-xs tracking-[0.2em] uppercase border border-red-300 text-red-500 hover:bg-red-500 hover:text-white transition-all duration-200"
            >
              {cancelling ? "Cancelando..." : "Cancelar reserva"}
            </button>
          )}
        </div>

        {/* Debug dev */}
        {import.meta.env.DEV && (
          <details className="mt-8 text-xs">
            <summary className="text-muted cursor-pointer">Ver datos crudos (dev)</summary>
            <pre className="bg-gray-100 p-4 mt-2 overflow-auto text-[10px]">
              {JSON.stringify({ booking, payments }, null, 2)}
            </pre>
          </details>
        )}
      </div>
    </div>
  );
}