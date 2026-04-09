import { useState } from "react";
import { Link } from "react-router-dom";
import useRoomController from "../../controllers/useRoomController";
import { RoomTypeLabel } from "../../models/Room";
import type { RoomTypeAvailabilityDto } from "../../models/Room";
import { RoomCardSkeleton } from "../components/Skeleton";
import { useTheme } from "../../context/ThemeContext";
import { useScrollAnimation } from "../../hooks/useScrollAnimation";
import DateRangePicker from "../components/DateRangePicker";
import SectionDivider from "../components/SectionDivider";
import { useSEO } from "../../hooks/useSEO";

/* ── DATA ─────────────────────────────────────────────── */
const roomImages: Record<number, string> = {
    0: "https://images.unsplash.com/photo-1566665797739-1674de7a421a?w=800&q=80",
    1: "https://images.unsplash.com/photo-1618773928121-c32242e63f39?w=800&q=80",
    2: "https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=800&q=80",
    3: "https://images.unsplash.com/photo-1578683010236-d716f9a3f461?w=800&q=80",
    4: "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?w=800&q=80",
};

const typeFilters = [
    { value: undefined, label: "Todas", icon: "◈" },
    { value: 0, label: "Single", icon: "◇" },
    { value: 1, label: "Double", icon: "◈" },
    { value: 2, label: "Suite", icon: "◉" },
    { value: 3, label: "Family", icon: "◎" },
    { value: 4, label: "Penthouse", icon: "⊕" },
];


/* ── Animated wrapper ─────────────────────────────────── */
function AnimSection({ children, className = "", anim = "anim-fade-up" }: {
    children: React.ReactNode;
    className?: string;
    anim?: string;
}) {
    const { ref, visible } = useScrollAnimation();
    return (
        <div ref={ref} className={`${anim} ${visible ? "visible" : ""} ${className}`}>
            {children}
        </div>
    );
}

function WhyAltiCard({ num, label, desc, icon, delay }: {
    num: string; label: string; desc: string; icon: string; delay: string;
}) {
    const { ref, visible } = useScrollAnimation();
    return (
        <div
            ref={ref}
            className={`anim-fade-up ${delay} ${visible ? "visible" : ""} bg-bg-base p-10 text-center hover:bg-bg-white transition-all duration-300 group cursor-default`}
        >
            <span className="text-primary text-2xl block mb-4 group-hover:scale-125 transition-transform duration-400">
                {icon}
            </span>
            <p className="font-serif text-4xl text-primary font-medium mb-1">{num}</p>
            <p className="text-[10px] tracking-[0.3em] uppercase text-text-sub font-semibold mb-3">{label}</p>
            <p className="text-text-sub text-xs leading-relaxed">{desc}</p>
        </div>
    );
}

/* ── Component ────────────────────────────────────────── */
export default function RoomsPage() {
    useSEO({ title: "Habitaciones", description: "Explore nuestras habitaciones de lujo." });

    const { theme } = useTheme();
    const {
        rooms, allRooms, loading, searched, error,
        typeFilter, searchRooms, filterByType, selectType,
    } = useRoomController();

    const [today] = useState(() => new Date().toISOString().split("T")[0]);
    const [tomorrow] = useState(() => {
        const d = new Date();
        d.setDate(d.getDate() + 1);
        return d.toISOString().split("T")[0];
    });

    const [checkIn, setCheckIn] = useState(today);
    const [checkOut, setCheckOut] = useState(tomorrow);

    const nights = Math.max(0, Math.ceil(
        (new Date(checkOut).getTime() - new Date(checkIn).getTime()) / 86400000
    ));

    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        searchRooms(checkIn, checkOut);
    };

    return (
        <div className="bg-bg-base">

            {/* ═══ HERO ═══════════════════════════════════ */}
            <section className="relative h-72 md:h-96 overflow-hidden">
                <img
                    src="https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=1800&q=80"
                    alt="Habitaciones"
                    className="w-full h-full object-cover"
                    style={{ transform: "scale(1.05)", transition: "transform 8s ease-out" }}
                />
                <div className="absolute inset-0 bg-gradient-to-b from-black/60 via-black/30 to-black/70" />

                <div className="absolute inset-0 flex flex-col items-center justify-center text-center text-white px-6"
                    style={{ animation: "fadeInUp 0.9s ease both" }}>
                    <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold mb-4">
                        ALTI Hotel
                    </span>
                    <h1 className="font-serif text-4xl md:text-6xl font-medium mb-4">
                        Nuestras Habitaciones
                    </h1>
                    <p className="text-white/60 text-sm tracking-widest max-w-md font-medium">
                        Desde suites presidenciales hasta confortables singles — cada espacio diseñado para usted
                    </p>
                </div>

                {/* Breadcrumb */}
                <div className="absolute bottom-6 left-6 flex items-center gap-2 text-white/40 text-[10px] tracking-widest uppercase font-medium">
                    <Link to="/" className="hover:text-white/70 transition-colors">Inicio</Link>
                    <span>›</span>
                    <span className="text-primary">Habitaciones</span>
                </div>
            </section>


            {/* ═══ FRANJA SEPARADORA ════════════════════════ */}
            <div className="bg-bg-base py-16 px-6">
                <div className="max-w-4xl mx-auto text-center">
                    <div className="flex items-center justify-center gap-4 mb-4">
                        <div className="w-12 h-px bg-primary" />
                        <span className="text-primary text-[25px] tracking-[0.3em] uppercase font-semibold">
                            Reserva tu estadía
                        </span>
                        <div className="w-12 h-px bg-primary" />
                    </div>
                    <p className="text-text-sub text-sm font-medium max-w-lg mx-auto">
                        Más de 48 habitaciones disponibles. Encuentra la que se adapta perfectamente a ti.
                    </p>
                </div>
            </div>

            {/* ═══ SEARCH ══════════════════════════════════ */}
            <section className="relative bg-bg-base pb-16">

                {/* Background imagen */}
                <div className="relative h-72 overflow-hidden">
                    <img
                        src="https://images.unsplash.com/photo-1618773928121-c32242e63f39?w=1800&q=80"
                        alt="Buscar habitación"
                        className="w-full h-full object-cover"
                        style={{ transform: "scale(1.05)" }}
                    />
                    <div className="absolute inset-0 bg-gradient-to-b from-black/50 via-black/50 to-black/80" />

                    {/* Texto */}
                    <div className="absolute inset-0 flex flex-col items-center justify-center text-center px-6">
                        <div className="flex items-center gap-4 mb-3">
                            <div className="w-8 h-px bg-primary" />
                            <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">
                                Disponibilidad
                            </span>
                            <div className="w-8 h-px bg-primary" />
                        </div>
                        <h2 className="font-serif text-3xl md:text-4xl font-medium text-white">
                            ¿Cuándo nos visita?
                        </h2>
                        <p className="text-white/50 text-sm mt-2 font-medium tracking-wide">
                            Seleccione sus fechas y explore nuestras habitaciones disponibles
                        </p>
                    </div>
                </div>

                {/* Floating card */}
                <div className="max-w-4xl mx-auto px-6 relative z-10" style={{ marginTop: "-48px" }}>
                    <AnimSection anim="anim-scale">
                        <form
                            onSubmit={handleSearch}
                            style={{
                                background: "var(--bg-300)",
                                border: "1px solid var(--bg-200)",
                                boxShadow: "0 20px 60px rgba(0,0,0,0.15), 0 4px 20px rgba(0,0,0,0.1)",
                            }}
                        >
                            {/* Top info bar */}
                            <div style={{ borderBottom: "1px solid var(--bg-200)", display: "flex" }}>
                                {[
                                    { icon: "◈", label: "Tipo de estadía", value: "Todas las habitaciones" },
                                    { icon: "◉", label: "Huéspedes", value: "1 – 6 personas" },
                                    { icon: "◎", label: "Duración", value: nights > 0 ? `${nights} noche${nights > 1 ? "s" : ""}` : "—" },
                                ].map((item, i, arr) => (
                                    <div
                                        key={i}
                                        className="flex-1 px-6 py-4 flex items-center gap-3"
                                        style={{ borderRight: i < arr.length - 1 ? "1px solid var(--bg-200)" : "none" }}
                                    >
                                        <span className="text-primary">{item.icon}</span>
                                        <div>
                                            <p className="text-[9px] tracking-[0.3em] uppercase text-text-sub font-semibold">{item.label}</p>
                                            <p className="text-text-main text-sm font-medium mt-0.5">{item.value}</p>
                                        </div>
                                    </div>
                                ))}
                            </div>

                            {/* DatePicker + button */}
                            <div className="p-8 flex flex-wrap gap-6 items-end">
                                <div className="flex-1 min-w-[300px]">
                                    <p className="text-[9px] tracking-[0.3em] uppercase text-text-sub font-semibold mb-3">
                                        Fechas de estadía
                                    </p>
                                    <DateRangePicker
                                        checkIn={checkIn}
                                        checkOut={checkOut}
                                        onChangeIn={setCheckIn}
                                        onChangeOut={setCheckOut}
                                        minDate={today}
                                        theme={theme}
                                    />
                                </div>
                                <div className="flex flex-col gap-3">
                                    <button
                                        type="submit"
                                        disabled={loading}
                                        className="btn-primary py-4 px-12 text-xs relative overflow-hidden group"
                                    >
                                        <span className="relative z-10 flex items-center gap-2">
                                            <span className="text-base">◈</span>
                                            {loading ? "Buscando..." : "Buscar habitaciones"}
                                        </span>
                                        <div className="absolute inset-0 -translate-x-full group-hover:translate-x-full transition-transform duration-700 bg-gradient-to-r from-transparent via-white/20 to-transparent" />
                                    </button>
                                    {(checkIn || checkOut) && (
                                        <button
                                            type="button"
                                            onClick={() => {
                                                const t = new Date().toISOString().split("T")[0];
                                                const tm = new Date(Date.now() + 86400000).toISOString().split("T")[0];
                                                setCheckIn(t);
                                                setCheckOut(tm);
                                            }}
                                            className="text-[9px] tracking-[0.2em] uppercase text-text-sub hover:text-primary transition-colors font-semibold text-center"
                                        >
                                            Limpiar fechas
                                        </button>
                                    )}
                                </div>
                            </div>

                            {/* Perks */}
                            <div
                                className="grid grid-cols-2 md:grid-cols-4 gap-4 px-8 py-4"
                                style={{ borderTop: "1px solid var(--bg-200)" }}
                            >
                                {[
                                    { icon: "◈", label: "Desayuno gourmet incluido en suites" },
                                    { icon: "◈", label: "Cancelación gratuita hasta 48h antes" },
                                    { icon: "◈", label: "Check-in express sin filas" },
                                    { icon: "◈", label: "Acceso al spa y piscina incluido" },
                                ].map((p, i) => (
                                    <div key={i} className="flex items-center gap-2">
                                        <span className="text-primary text-xs flex-shrink-0">{p.icon}</span>
                                        <span className="text-text-sub text-[10px] font-medium leading-tight">{p.label}</span>
                                    </div>
                                ))}
                            </div>
                        </form>
                    </AnimSection>
                </div>
            </section>

            {/* ═══ RESULTS ═════════════════════════════════ */}
            <section className="bg-bg-card py-16 px-6 min-h-[400px]">
                <div className="max-w-7xl mx-auto">

                    {/* Empty state */}
                    {!searched && !loading && (
                        <AnimSection className="text-center py-20">
                            <div className="w-20 h-20 rounded-full bg-primary/10 border border-primary/20 flex items-center justify-center mx-auto mb-6">
                                <span className="text-primary text-3xl">◈</span>
                            </div>
                            <p className="font-serif text-3xl text-text-sub/50 font-medium mb-2">
                                Elija sus fechas
                            </p>
                            <p className="text-text-sub text-sm tracking-widest uppercase font-medium">
                                Para ver habitaciones disponibles
                            </p>
                        </AnimSection>
                    )}

                    {/* Loading skeletons */}
                    {loading && (
                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6"
                            style={{ animation: "fadeInUp 0.4s ease both" }}>
                            {[1, 2, 3, 4, 5, 6].map(i => <RoomCardSkeleton key={i} />)}
                        </div>
                    )}

                    {/* No results */}
                    {!loading && searched && allRooms.length === 0 && (
                        <AnimSection className="text-center py-20">
                            <div className="w-20 h-20 rounded-full bg-primary/10 border border-primary/20 flex items-center justify-center mx-auto mb-6">
                                <span className="text-primary text-3xl">⊘</span>
                            </div>
                            <p className="font-serif text-3xl text-text-sub/50 font-medium mb-2">
                                Sin disponibilidad
                            </p>
                            <p className="text-text-sub text-sm tracking-widest uppercase font-medium mb-8">
                                Intente con otras fechas
                            </p>
                            <button
                                onClick={() => { setCheckIn(today); setCheckOut(tomorrow); }}
                                className="btn-outline px-8 py-3"
                            >
                                Restablecer fechas
                            </button>
                        </AnimSection>
                    )}

                    {/* Results */}
                    {!loading && allRooms.length > 0 && (
                        <>
                            {/* Header + filters */}
                            <AnimSection className="mb-10">
                                <div className="flex flex-col md:flex-row md:items-center justify-between gap-6 mb-6">
                                    <div>
                                        <div className="flex items-center gap-3 mb-1">
                                            <div className="w-6 h-px bg-primary" />
                                            <span className="text-primary text-[10px] tracking-[0.4em] uppercase font-semibold">
                                                Disponibles
                                            </span>
                                        </div>
                                        <p className="font-serif text-2xl text-text-main font-medium">
                                            {rooms.length} tipo{rooms.length !== 1 ? "s" : ""} de habitación disponible{rooms.length !== 1 ? "s" : ""}
                                            {nights > 0 && (
                                                <span className="text-primary"> · {nights} noche{nights !== 1 ? "s" : ""}</span>
                                            )}
                                        </p>
                                    </div>
                                </div>

                                {/* Type filters */}
                                <div className="flex flex-wrap gap-2">
                                    {typeFilters.map(f => (
                                        <button
                                            key={String(f.value)}
                                            onClick={() => filterByType(f.value)}
                                            className={`flex items-center gap-2 text-[10px] tracking-[0.2em] uppercase px-5 py-2.5 border font-semibold transition-all duration-300 ${typeFilter === f.value
                                                ? "bg-primary border-primary text-white scale-105 shadow-lg shadow-primary/20"
                                                : "border-bg-base text-text-sub hover:border-primary hover:text-primary"
                                                }`}
                                        >
                                            <span className={`text-xs ${typeFilter === f.value ? "text-white" : "text-primary"}`}>
                                                {f.icon}
                                            </span>
                                            {f.label}
                                        </button>
                                    ))}
                                </div>
                            </AnimSection>

                            {/* No results for filter */}
                            {rooms.length === 0 ? (
                                <AnimSection className="text-center py-16">
                                    <p className="font-serif text-2xl text-text-sub/40 font-medium">
                                        Sin resultados para este tipo
                                    </p>
                                    <button onClick={() => filterByType(undefined)} className="btn-outline px-6 py-2.5 mt-6 text-xs">
                                        Ver todas
                                    </button>
                                </AnimSection>
                            ) : (
                                <RoomGrid
                                    rooms={rooms}
                                    nights={nights}
                                    onBook={(roomType) => selectType(roomType, checkIn, checkOut)}
                                />
                            )}
                        </>
                    )}

                    {error && (
                        <div className="border-l-2 border-red-400 pl-4 py-3 mb-8">
                            <p className="text-red-500 text-xs tracking-wide font-medium">{error}</p>
                        </div>
                    )}
                </div>
            </section>

            <SectionDivider from="var(--bg-200)" to="var(--bg-100)" />

            {/* ═══ WHY ALTI ════════════════════════════════ */}
            <section className="py-24 px-6 bg-bg-base">
                <div className="max-w-7xl mx-auto">

                    <AnimSection className="text-center mb-16">
                        <div className="flex items-center justify-center gap-4 mb-4">
                            <div className="w-8 h-px bg-primary" />
                            <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">
                                Por qué elegirnos
                            </span>
                            <div className="w-8 h-px bg-primary" />
                        </div>
                        <h2 className="font-serif text-4xl md:text-5xl text-text-main font-medium">
                            Más allá de una habitación
                        </h2>
                    </AnimSection>

                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-px bg-bg-card">
                        {[
                            { num: "48", label: "Habitaciones", desc: "Desde suites presidenciales hasta confortables rooms para viajeros solos.", icon: "◈", delay: "delay-100" },
                            { num: "5★", label: "Calificación", desc: "Reconocidos como el mejor hotel boutique del Caribe por 3 años consecutivos.", icon: "★", delay: "delay-200" },
                            { num: "24/7", label: "Servicio", desc: "Nuestro equipo de concierge disponible a cualquier hora para lo que necesite.", icon: "◉", delay: "delay-300" },
                            { num: "100%", label: "Satisfacción", desc: "Más de 50,000 huéspedes satisfechos avalan nuestra promesa de excelencia.", icon: "◎", delay: "delay-400" },
                        ].map((item, i) => (
                            <WhyAltiCard key={i} {...item} />
                        ))}
                    </div>
                </div>
            </section>

            <SectionDivider from="var(--bg-100)" to="var(--bg-200)" flip />

            {/* ═══ CTA ══════════════════════════════════════ */}
            <section className="relative py-28 px-6 overflow-hidden">
                <img
                    src="https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=1800&q=80"
                    alt="Suite"
                    className="absolute inset-0 w-full h-full object-cover"
                />
                <div className="absolute inset-0 bg-primary/80" />

                {/* Animated rings */}
                <div className="absolute inset-0 flex items-center justify-center pointer-events-none overflow-hidden">
                    {[1, 2, 3].map(i => (
                        <div key={i}
                            className="absolute rounded-full border border-white/10"
                            style={{
                                width: `${i * 260}px`,
                                height: `${i * 260}px`,
                                animation: `ripple ${3 + i}s ease-out infinite`,
                                animationDelay: `${i * 0.8}s`,
                            }}
                        />
                    ))}
                </div>

                <AnimSection className="relative text-center text-white max-w-2xl mx-auto">
                    <span className="text-white/60 text-[10px] tracking-[0.5em] uppercase font-semibold block mb-6">
                        ¿No encontró lo que busca?
                    </span>
                    <h2 className="font-serif text-4xl md:text-5xl font-medium mb-6 leading-tight">
                        Háblenos de sus<br />
                        <span className="italic text-white/80">preferencias</span>
                    </h2>
                    <p className="text-white/60 text-sm mb-10 max-w-md mx-auto font-medium">
                        Nuestro equipo de concierge puede preparar una oferta personalizada
                        exactamente adaptada a sus necesidades.
                    </p>
                    <div className="flex flex-col sm:flex-row gap-4 justify-center">
                        <Link to="/contact" className="btn-primary px-10 py-4 text-xs">
                            Contactar concierge
                        </Link>
                        <Link to="/services"
                            className="border border-white/40 text-white text-xs tracking-[0.2em] uppercase font-semibold px-10 py-4 hover:bg-white/10 transition-colors duration-300">
                            Ver servicios
                        </Link>
                    </div>
                </AnimSection>
            </section>

            <style>{`
        @keyframes fadeInUp {
          from { opacity: 0; transform: translateY(24px); }
          to   { opacity: 1; transform: translateY(0);    }
        }
        @keyframes ripple {
          0%   { transform: scale(0.6); opacity: 0.5; }
          100% { transform: scale(2.2); opacity: 0;   }
        }
      `}</style>
        </div>
    );
}

/* ── Room Grid with stagger ───────────────────────────── */
function RoomGrid({ rooms, nights, onBook }: {
    rooms: RoomTypeAvailabilityDto[];
    nights: number;
    onBook: (roomType: RoomTypeAvailabilityDto) => void;
}) {
    const { ref, visible } = useScrollAnimation(0.05);
    const delays = ["delay-100", "delay-200", "delay-300", "delay-400", "delay-500", "delay-600"];

    return (
        <div ref={ref} className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {rooms.map((roomType, i) => (
                <div
                    key={roomType.type}
                    className={`anim-fade-up ${delays[i % 6] ?? "delay-600"} ${visible ? "visible" : ""}`}
                >
                    <RoomTypeCard roomType={roomType} nights={nights} onBook={() => onBook(roomType)} />
                </div>
            ))}
        </div>
    );
}

/* ── Room Type Card ───────────────────────────────────── */
function RoomTypeCard({ roomType, nights, onBook }: {
    roomType: RoomTypeAvailabilityDto;
    nights: number;
    onBook: () => void;
}) {
    const [hovered, setHovered] = useState(false);

    return (
        <div
            className="group bg-bg-white border border-bg-card overflow-hidden transition-all duration-500 hover:shadow-2xl"
            style={{ transform: hovered ? "translateY(-4px)" : "translateY(0)", transition: "transform 0.4s ease, box-shadow 0.4s ease" }}
            onMouseEnter={() => setHovered(true)}
            onMouseLeave={() => setHovered(false)}
        >
            {/* Image */}
            <div className="relative h-60 overflow-hidden">
                <img
                    src={roomImages[roomType.type] ?? roomImages[0]}
                    alt={roomType.typeName}
                    className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-110"
                />
                <div className="absolute inset-0 bg-gradient-to-t from-black/50 to-transparent" />

                {/* Type badge */}
                <span className="absolute top-4 left-4 bg-primary text-white text-[9px] tracking-[0.2em] uppercase px-3 py-1.5 font-semibold">
                    {RoomTypeLabel[roomType.type]}
                </span>

                {/* Availability badge */}
                <span className="absolute top-4 right-4 bg-black/60 text-white text-[9px] tracking-[0.15em] uppercase px-3 py-1.5 font-semibold">
                    {roomType.availableRooms} disponible{roomType.availableRooms > 1 ? "s" : ""}
                </span>

                {/* Total price overlay on hover */}
                {nights > 0 && (
                    <div className={`absolute bottom-4 right-4 bg-bg-base/90 px-3 py-2 text-right transition-all duration-400 ${hovered ? "opacity-100 translate-y-0" : "opacity-0 translate-y-3"}`}>
                        <p className="text-[9px] tracking-widest uppercase text-text-sub font-semibold">Desde</p>
                        <p className="font-serif text-xl text-primary font-medium">${roomType.minPrice * nights}</p>
                    </div>
                )}
            </div>

            {/* Content */}
            <div className="p-6">
                <div className="flex justify-between items-start mb-3">
                    <h3 className="font-serif text-xl text-text-main font-medium">
                        {RoomTypeLabel[roomType.type]}
                    </h3>
                    <div className="text-right">
                        <p className="text-primary font-bold text-lg">
                            {roomType.minPrice === roomType.maxPrice
                                ? `$${roomType.minPrice}`
                                : `$${roomType.minPrice} — $${roomType.maxPrice}`}
                        </p>
                        <p className="text-[10px] text-text-sub tracking-widest uppercase font-medium">/noche</p>
                    </div>
                </div>

                {/* Details */}
                <div className="flex gap-4 text-[10px] tracking-widest uppercase text-text-sub mb-4 font-semibold">
                    <span className="flex items-center gap-1">
                        <span className="text-primary text-xs">◈</span> Hasta {roomType.maxCapacity} persona{roomType.maxCapacity > 1 ? "s" : ""}
                    </span>
                    <span>·</span>
                    <span className="flex items-center gap-1">
                        <span className="text-primary text-xs">◈</span> {roomType.availableRooms}/{roomType.totalRooms} disponible{roomType.availableRooms > 1 ? "s" : ""}
                    </span>
                </div>

                {roomType.sampleDescription && (
                    <p className="text-text-sub text-sm leading-relaxed mb-5 line-clamp-2">
                        {roomType.sampleDescription}
                    </p>
                )}

                {/* Nights bar */}
                {nights > 0 && (
                    <div className="bg-bg-base px-4 py-2.5 mb-5 flex justify-between items-center border-l-2 border-primary">
                        <span className="text-[10px] tracking-widest uppercase text-text-sub font-semibold">
                            {nights} noche{nights > 1 ? "s" : ""}
                        </span>
                        <span className="font-serif text-lg text-text-main font-medium">
                            desde ${roomType.minPrice * nights}
                        </span>
                    </div>
                )}

                {/* CTA */}
                <button
                    onClick={onBook}
                    className="btn-primary w-full py-3 text-xs relative overflow-hidden group/btn"
                >
                    <span className="relative z-10">Reservar ahora</span>
                    <div className="absolute inset-0 -translate-x-full group-hover/btn:translate-x-full transition-transform duration-700 bg-gradient-to-r from-transparent via-white/20 to-transparent" />
                </button>
            </div>
        </div>
    );
}