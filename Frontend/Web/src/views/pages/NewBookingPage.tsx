import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import useBookingController from "../../controllers/useBookingController";
import { RoomTypeLabel } from "../../models/Room";
import type { RoomTypeAvailabilityDto } from "../../models/Room";

export default function NewBookingPage() {
  const { user } = useAuth();
  const navigate = useNavigate();
  const { state } = useLocation() as {
    state: { roomType: RoomTypeAvailabilityDto; checkIn: string; checkOut: string };
  };
  const { createBookingByType, loading, error } = useBookingController();
  const [notes, setNotes] = useState("");

  if (!state?.roomType) { navigate("/rooms"); return null; }

  const { roomType, checkIn, checkOut } = state;
  const nights = Math.ceil(
    (new Date(checkOut).getTime() - new Date(checkIn).getTime()) / 86400000
  );
  const total = roomType.minPrice * nights;

  const handleConfirm = async () => {
    if (!user) { navigate("/login"); return; }
    const booking = await createBookingByType({
      guestId: user.id,
      roomType: roomType.type,
      checkInDate: checkIn,
      checkOutDate: checkOut,
      notes: notes || undefined,
    });
    if (booking) navigate(`/bookings/${booking.id}/services`, { state: { booking, total } });
  };

  return (
    <div className="min-h-screen bg-bg-base pt-20">
      <div className="max-w-5xl mx-auto px-6 py-16">

        <button onClick={() => navigate("/rooms")}
          className="text-[10px] tracking-[0.2em] uppercase text-text-sub hover:text-primary transition-colors flex items-center gap-2 mb-10">
          ← Volver a habitaciones
        </button>

        <div className="flex items-center gap-6 mb-12">
          <div className="h-px flex-1 bg-bg-card" />
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Confirmar Reserva</span>
          <div className="h-px flex-1 bg-bg-card" />
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-10">

          {/* Summary */}
          <div className="bg-bg-white border border-bg-card p-8">
            <h2 className="font-serif text-2xl text-text-main mb-8">Resumen de su reserva</h2>
            <div className="space-y-5">
              {[
                { label: "Tipo de habitación", value: RoomTypeLabel[roomType.type] ?? roomType.typeName },
                { label: "Disponibles", value: `${roomType.availableRooms} habitación${roomType.availableRooms > 1 ? "es" : ""}` },
                { label: "Capacidad máxima", value: `${roomType.maxCapacity} persona${roomType.maxCapacity > 1 ? "s" : ""}` },
                { label: "Check-In", value: checkIn },
                { label: "Check-Out", value: checkOut },
                { label: "Noches", value: `${nights}` },
                { label: "Tarifa desde", value: `$${roomType.minPrice}/noche` },
              ].map(row => (
                <div key={row.label} className="flex justify-between items-center pb-4 border-b border-bg-card">
                  <span className="text-[10px] tracking-[0.2em] uppercase text-text-sub">{row.label}</span>
                  <span className="text-sm text-text-main font-medium">{row.value}</span>
                </div>
              ))}
              <div className="flex justify-between items-center pt-2">
                <span className="text-[10px] tracking-[0.2em] uppercase text-text-main font-medium">Total estimado</span>
                <div className="text-right">
                  <p className="font-serif text-3xl text-primary">${total}</p>
                  <p className="text-[10px] text-text-sub tracking-widest uppercase">USD</p>
                </div>
              </div>
            </div>
            <div className="mt-8 bg-bg-base p-4">
              <p className="text-[10px] tracking-wide text-text-sub leading-relaxed">
                ✦ El hotel asignará la mejor habitación disponible del tipo seleccionado<br />
                ✦ Cancelación gratuita hasta 48h antes del check-in<br />
                ✦ Confirmación inmediata por correo electrónico
              </p>
            </div>
          </div>

          {/* Notes + user */}
          <div className="flex flex-col gap-6">
            <div className="bg-bg-white border border-bg-card p-8">
              <h2 className="font-serif text-2xl text-text-main mb-2">Solicitudes especiales</h2>
              <p className="text-text-sub text-xs tracking-wide mb-6">Indíquenos cualquier preferencia o necesidad especial</p>
              <label className="block text-[10px] tracking-[0.2em] uppercase text-text-sub mb-3">Notas (opcional)</label>
              <textarea value={notes} onChange={e => setNotes(e.target.value)} rows={5}
                placeholder="Llegada tardía, cama extra, ocasión especial..."
                className="w-full border-0 border-b-2 border-bg-card bg-transparent outline-none resize-none text-sm text-text-main placeholder:text-text-sub focus:border-primary transition-colors duration-200 font-sans font-light pb-2" />
            </div>

            {error && (
              <div className="border-l-2 border-red-400 pl-4 py-3">
                <p className="text-red-500 text-xs tracking-wide">{error}</p>
              </div>
            )}

            <div className="bg-bg-white border border-bg-card p-6">
              <h2 className="font-serif text-xl text-text-main mb-2">Sus datos</h2>
              <div className="space-y-1 text-xs text-text-sub">
                <p><span className="tracking-widest uppercase">Nombre: </span>{user?.fullName}</p>
                <p><span className="tracking-widest uppercase">Email: </span>{user?.email}</p>
              </div>
            </div>

            <div className="flex gap-4">
              <button onClick={() => navigate("/rooms")} className="btn-outline flex-1 py-4">Cancelar</button>
              <button onClick={handleConfirm} disabled={loading} className="btn-primary flex-1 py-4">
                {loading ? "Procesando..." : "Continuar al pago"}
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}