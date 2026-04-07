import { useLocation, Link } from "react-router-dom";
import useBookingController from "../../controllers/useBookingController";
import { BookingStatusLabel, BookingStatusColor } from "../../models/Booking";
import { useAuth } from "../../context/AuthContext";
import { BookingCardSkeleton } from "../components/Skeleton";
import BookingSearch from "../components/BookingSearch";
import { useSEO } from "../../hooks/useSEO";

const statusIcon: Record<number,string> = { 0:"◌",1:"✓",2:"⬤",3:"◎",4:"✕",5:"⊘" };

export default function MyBookingsPage() {
  useSEO({ title: "Mis reservas", description: "Gestione sus reservas en ALTI Hotel." });

  const { user }   = useAuth();
  const location   = useLocation();
  const success    = (location.state as any)?.success ?? false;
  const { bookings, loading, error, cancelBooking } = useBookingController();

  if (!user) return (
    <div className="min-h-screen bg-bg-base pt-20 flex items-center justify-center">
      <div className="text-center">
        <p className="font-serif text-3xl text-text-sub mb-2">Acceso requerido</p>
        <p className="text-text-sub text-xs tracking-widest uppercase mb-8">Inicie sesión para ver sus reservas</p>
        <Link to="/login" className="btn-primary">Iniciar Sesión</Link>
      </div>
    </div>
  );

  if (loading) return (
    <div className="min-h-screen bg-bg-base pt-20">
      <div className="bg-bg-card py-14 px-6">
        <div className="max-w-6xl mx-auto">
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Mi cuenta</span>
          <h1 className="font-serif text-4xl md:text-5xl text-text-main mt-2">Mis Reservas</h1>
        </div>
      </div>
      <div className="max-w-6xl mx-auto px-6 py-12 space-y-4">
        {[1,2,3].map(i => <BookingCardSkeleton key={i} />)}
      </div>
    </div>
  );

  return (
    <div className="min-h-screen bg-bg-base pt-20">
      <div className="bg-bg-card py-14 px-6">
        <div className="max-w-6xl mx-auto">
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Mi cuenta</span>
          <h1 className="font-serif text-4xl md:text-5xl text-text-main mt-2">Mis Reservas</h1>
          <p className="text-text-sub text-xs tracking-widest mt-2">
            {user.fullName} · {bookings.length} reserva{bookings.length!==1?"s":""}
          </p>
        </div>
      </div>

      <div className="max-w-6xl mx-auto px-6 py-12">

        <div className="bg-bg-white border border-bg-card p-6 mb-8">
          <BookingSearch />
        </div>

        {success && (
          <div className="border-l-4 border-primary pl-6 py-4 bg-bg-white mb-8">
            <p className="text-[10px] tracking-[0.2em] uppercase text-primary mb-1">Pago registrado</p>
            <p className="text-text-main text-sm">Su reserva está pendiente de confirmación. Recibirá un correo en breve.</p>
          </div>
        )}

        {error && (
          <div className="border-l-2 border-red-400 pl-4 py-3 mb-8">
            <p className="text-red-500 text-xs tracking-wide">{error}</p>
          </div>
        )}

        {bookings.length === 0 ? (
          <div className="text-center py-24">
            <p className="font-serif text-5xl text-text-sub/20 mb-4">◎</p>
            <p className="font-serif text-3xl text-text-sub mb-2">Sin reservas</p>
            <p className="text-text-sub text-xs tracking-widest uppercase mb-10">Aún no ha realizado ninguna reserva</p>
            <Link to="/rooms" className="btn-primary">Explorar habitaciones</Link>
          </div>
        ) : (
          <div className="space-y-4">
            {bookings.map(b => (
              <div key={b.id} className="bg-bg-white border border-bg-card hover:shadow-md transition-all duration-300">
                <div className="p-6 md:p-8">
                  <div className="flex flex-col md:flex-row justify-between gap-6">
                    <div className="flex-1">
                      <div className="flex items-center gap-4 mb-4">
                        <span className="text-primary text-xl">{statusIcon[b.status]}</span>
                        <div>
                          <Link to={`/bookings/${b.id}`}
                            className="font-serif text-2xl text-text-main hover:text-primary transition-colors">
                            {b.code}
                          </Link>
                          <div className="mt-1">
                            <span className={`text-[9px] tracking-[0.2em] uppercase px-2 py-0.5 ${BookingStatusColor[b.status]}`}>
                              {BookingStatusLabel[b.status]}
                            </span>
                          </div>
                        </div>
                      </div>

                      <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
                        {[
                          { label:"Check-In",  value:b.checkInDate  },
                          { label:"Check-Out", value:b.checkOutDate },
                          { label:"Noches",    value:`${b.nights}`  },
                          { label:"Total",     value:`$${b.totalPrice}`, highlight:true },
                        ].map(row => (
                          <div key={row.label}>
                            <p className="text-[9px] tracking-[0.2em] uppercase text-text-sub mb-1">{row.label}</p>
                            <p className={`text-sm ${row.highlight?"font-serif text-xl text-primary":"text-text-main font-medium"}`}>
                              {row.value}
                            </p>
                          </div>
                        ))}
                      </div>

                      {b.notes && (
                        <p className="text-text-sub text-xs mt-4 italic border-l border-primary/40 pl-3">{b.notes}</p>
                      )}
                    </div>

                    {(b.status===0||b.status===1) && (
                      <div className="flex items-center">
                        <button onClick={()=>cancelBooking(b.id)} className="btn-danger">
                          Cancelar reserva
                        </button>
                      </div>
                    )}
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}

        <div className="text-center mt-16 pt-10 border-t border-bg-card">
          <Link to="/rooms" className="btn-outline">Realizar nueva reserva</Link>
        </div>
      </div>
    </div>
  );
}