import { useState, useEffect, useRef } from "react";
import { Link } from "react-router-dom";
import { useSEO } from "../../hooks/useSEO";
import { useScrollAnimation } from "../../hooks/useScrollAnimation";
import SectionDivider from "../components/SectionDivider";

/* ── DATA ─────────────────────────────────────────────── */
const slides = [
    { img: "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=1800&q=80", tag: "Bienvenido a ALTI Hotel", title: "Donde el lujo\nse convierte\nen hogar", sub: "Una experiencia sin igual en el corazón de la ciudad" },
    { img: "https://images.unsplash.com/photo-1571896349842-33c89424de2d?w=1800&q=80", tag: "Nuestras Suites", title: "Espacios diseñados\npara la perfección", sub: "Cada detalle cuidado para tu máximo confort" },
    { img: "https://images.unsplash.com/photo-1540541338287-41700207dee6?w=1800&q=80", tag: "Gastronomía", title: "Una cocina que\ndespierta los sentidos", sub: "Alta cocina internacional en un ambiente excepcional" },
];

const stats = [
    { num: "12+", label: "Años de excelencia" },
    { num: "48", label: "Habitaciones" },
    { num: "5★", label: "Calificación" },
    { num: "50k+", label: "Huéspedes" },
    { num: "24/7", label: "Servicio" },
    { num: "3", label: "Restaurantes" },
];

const rooms = [
    { type: "Suite Presidencial", price: 650, img: "https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=800&q=80", tag: "Más popular", desc: "Vista panorámica, jacuzzi privado, mayordomo personal." },
    { type: "Suite Deluxe", price: 350, img: "https://images.unsplash.com/photo-1618773928121-c32242e63f39?w=800&q=80", tag: "Favorita", desc: "Terraza privada, sala de estar, elegancia moderna." },
    { type: "Habitación Double", price: 180, img: "https://images.unsplash.com/photo-1566665797739-1674de7a421a?w=800&q=80", tag: "Mejor valor", desc: "Confort premium con vistas al jardín interior." },
];

const gallery = [
    { img: "https://images.unsplash.com/photo-1584132967334-10e028bd69f7?w=800&q=80", label: "Spa & Wellness", span: "col-span-2 row-span-2" },
    { img: "https://images.unsplash.com/photo-1414235077428-338989a2e8c0?w=800&q=80", label: "Alta Cocina", span: "col-span-1 row-span-1" },
    { img: "https://images.unsplash.com/photo-1540555700478-4be289fbecef?w=800&q=80", label: "Piscina Infinita", span: "col-span-1 row-span-1" },
    { img: "https://images.unsplash.com/photo-1563911302283-d2bc129e7570?w=800&q=80", label: "Terrazas", span: "col-span-1 row-span-1" },
    { img: "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?w=800&q=80", label: "Lobby", span: "col-span-2 row-span-1" },
    { img: "https://images.unsplash.com/photo-1578683010236-d716f9a3f461?w=800&q=80", label: "Family Suite", span: "col-span-1 row-span-1" },
];

const amenities = [
    { icon: "◈", title: "Spa de lujo", desc: "6 cabinas privadas, sauna finlandesa, piscina termal y tratamientos exclusivos." },
    { icon: "◈", title: "Alta cocina", desc: "3 restaurantes: mediterráneo, fusión asiática y cocina dominicana de autor." },
    { icon: "◈", title: "Piscina infinita", desc: "550 m² con vistas al horizonte, bar acuático y zona de relax exclusiva." },
    { icon: "◈", title: "Business Center", desc: "5 salas de reuniones, equipamiento AV de última generación y wifi 10 Gbps." },
    { icon: "◈", title: "Fitness Premium", desc: "Gimnasio 24h, clases de yoga, pilates y entrenadores personales certificados." },
    { icon: "◈", title: "Concierge 24/7", desc: "Equipo dedicado para traslados, reservas, tours y solicitudes especiales." },
    { icon: "◈", title: "Kids Club", desc: "Espacio supervisado con actividades para niños de 3 a 12 años." },
    { icon: "◈", title: "Helipad privado", desc: "Acceso exclusivo para llegadas y salidas en helicóptero." },
];

const experiences = [
    { img: "https://images.unsplash.com/photo-1599458252573-56ae36120de1?w=800&q=80", title: "City Tours Privados", desc: "Recorra lo mejor de Santo Domingo con guía experto y vehículo de lujo.", price: "Desde $90" },
    { img: "https://images.unsplash.com/photo-1414235077428-338989a2e8c0?w=800&q=80", title: "Cena bajo las Estrellas", desc: "Mesa privada en nuestra terraza con menú degustación y sommelier.", price: "Desde $120" },
    { img: "https://images.unsplash.com/photo-1540555700478-4be289fbecef?w=800&q=80", title: "Ritual Spa Couple", desc: "90 minutos de masajes en pareja con aromaterapia y champagne.", price: "Desde $180" },
];

const reviews = [
    { name: "María González", country: "España", rating: 5, text: "Una experiencia absolutamente excepcional. El servicio es impecable y las habitaciones transmiten un lujo genuino." },
    { name: "James Thompson", country: "Reino Unido", rating: 5, text: "The best hotel I've stayed in Latin America. The attention to detail is remarkable and the staff made us feel truly special." },
    { name: "Sophie Leclerc", country: "Francia", rating: 5, text: "Un séjour inoubliable. La cuisine était extraordinaire et le spa nous a complètement revitalisés. Un vrai bijou." },
    { name: "Carlos Rodríguez", country: "Colombia", rating: 5, text: "Celebramos nuestro aniversario aquí y fue perfecto. Cada detalle estuvo cuidado al máximo. El personal es excepcional." },
];

/* ── Animated Section Wrapper ─────────────────────────── */
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

/* ── Stagger children ─────────────────────────────────── */
function StaggerList({ items, className = "", renderItem }: {
    items: any[];
    className?: string;
    renderItem: (item: any, i: number) => React.ReactNode;
}) {
    const { ref, visible } = useScrollAnimation();
    const delays = ["delay-100", "delay-200", "delay-300", "delay-400", "delay-500", "delay-600", "delay-700", "delay-800"];
    return (
        <div ref={ref} className={className}>
            {items.map((item, i) => (
                <div key={i} className={`anim-fade-up ${delays[i] ?? "delay-800"} ${visible ? "visible" : ""}`}>
                    {renderItem(item, i)}
                </div>
            ))}
        </div>
    );
}

/* ── Component ────────────────────────────────────────── */
export default function HomePage() {
    useSEO({ title: "Inicio", description: "ALTI Hotel — Lujo, confort y servicio excepcional en Santo Domingo." });

    const [current, setCurrent] = useState(0);
    const [review, setReview] = useState(0);
    const [lightbox, setLightbox] = useState<number | null>(null);
    const timerRef = useRef<ReturnType<typeof setInterval> | null>(null);

    // Parallax
    const heroRef = useRef<HTMLDivElement>(null);
    useEffect(() => {
        const onScroll = () => {
            if (!heroRef.current) return;
            const y = window.scrollY;
            const imgs = heroRef.current.querySelectorAll<HTMLImageElement>(".parallax-img");
            imgs.forEach(img => { img.style.transform = `translateY(${y * 0.25}px)`; });
        };
        window.addEventListener("scroll", onScroll, { passive: true });
        return () => window.removeEventListener("scroll", onScroll);
    }, []);

    useEffect(() => {
        timerRef.current = setInterval(() => setCurrent(p => (p + 1) % slides.length), 6000);
        return () => { if (timerRef.current) clearInterval(timerRef.current); };
    }, []);

    useEffect(() => {
        const t = setInterval(() => setReview(p => (p + 1) % reviews.length), 5000);
        return () => clearInterval(t);
    }, []);

    const goTo = (i: number) => {
        setCurrent(i);
        if (timerRef.current) clearInterval(timerRef.current);
        timerRef.current = setInterval(() => setCurrent(p => (p + 1) % slides.length), 6000);
    };

    return (
        <div className="bg-bg-base overflow-x-hidden">

            {/* ═══ 1. HERO ═══════════════════════════════════ */}
            <section ref={heroRef} className="relative h-screen min-h-[600px] overflow-hidden">
                {slides.map((slide, i) => (
                    <div key={i} className={`absolute inset-0 transition-opacity duration-1000 ${i === current ? "opacity-100" : "opacity-0"}`}>
                        <img
                            src={slide.img}
                            alt={slide.title}
                            className="w-full h-full object-cover parallax-img"
                            style={{ transformOrigin: "center", transform: "translateY(0)" }}
                        />
                        <div className="absolute inset-0 bg-gradient-to-b from-black/60 via-black/25 to-black/75" />
                    </div>
                ))}

                <div className="absolute inset-0 flex flex-col items-center justify-center text-center text-white px-6">
                    <span key={`tag-${current}`} className="text-primary text-xs tracking-[0.5em] uppercase mb-6 font-semibold"
                        style={{ animation: "fadeInUp 0.9s ease both" }}>
                        {slides[current].tag}
                    </span>
                    <h1 key={`h-${current}`}
                        className="font-serif text-5xl md:text-7xl lg:text-8xl font-medium leading-tight mb-6 max-w-5xl"
                        style={{ animation: "fadeInUp 0.9s 0.15s ease both" }}>
                        {slides[current].title.split("\n").map((l, i) => <span key={i} className="block">{l}</span>)}
                    </h1>
                    <p key={`sub-${current}`}
                        className="text-white/70 text-sm md:text-base tracking-widest max-w-xl mb-12 font-medium"
                        style={{ animation: "fadeInUp 0.9s 0.3s ease both" }}>
                        {slides[current].sub}
                    </p>
                    <div className="flex flex-col sm:flex-row gap-4"
                        style={{ animation: "fadeInUp 0.9s 0.45s ease both" }}>
                        <Link to="/rooms" className="btn-primary px-10 py-4 text-xs">Explorar Habitaciones</Link>
                        <Link to="/services" className="btn-outline-white px-10 py-4 text-xs">Nuestros Servicios</Link>
                    </div>
                </div>

                {/* Dots */}
                <div className="absolute bottom-10 left-1/2 -translate-x-1/2 flex gap-3">
                    {slides.map((_, i) => (
                        <button key={i} onClick={() => goTo(i)}
                            className={`transition-all duration-500 rounded-full ${i === current ? "w-8 h-1.5 bg-primary" : "w-1.5 h-1.5 bg-white/40 hover:bg-white/70"
                                }`} />
                    ))}
                </div>

                {/* Scroll hint */}
                <div className="absolute bottom-10 right-10 hidden md:flex flex-col items-center gap-2">
                    <span className="text-white/30 text-[9px] tracking-[0.4em] uppercase font-medium"
                        style={{ writingMode: "vertical-rl" }}>Scroll</span>
                    <div className="w-px h-14 bg-white/15 relative overflow-hidden">
                        <div className="absolute top-0 w-full h-1/2 bg-primary"
                            style={{ animation: "scrollDrop 1.8s ease-in-out infinite" }} />
                    </div>
                </div>
            </section>

            {/* ═══ DIVIDER ══════════════════════════════════ */}
            <SectionDivider from="transparent" to="var(--bg-200)" />

            {/* ═══ 2. STATS MARQUEE ═════════════════════════ */}
            <section className="bg-bg-card py-5 overflow-hidden border-y border-bg-base">
                <div className="flex gap-0 whitespace-nowrap" style={{ animation: "marquee 22s linear infinite" }}>
                    {[...stats, ...stats, ...stats].map((s, i) => (
                        <div key={i} className="flex items-center gap-6 px-10">
                            <div className="text-center">
                                <p className="font-serif text-primary text-2xl font-medium leading-none">{s.num}</p>
                                <p className="text-text-sub text-[9px] tracking-[0.3em] uppercase font-semibold mt-1">{s.label}</p>
                            </div>
                            <span className="text-primary/40 text-xl">◈</span>
                        </div>
                    ))}
                </div>
            </section>

            {/* ═══ DIVIDER ══════════════════════════════════ */}
            <SectionDivider from="var(--bg-200)" to="var(--bg-100)" />

            {/* ═══ 3. INTRO EDITORIAL ═══════════════════════ */}
            <section className="py-32 px-6 bg-bg-base">
                <div className="max-w-7xl mx-auto grid grid-cols-1 lg:grid-cols-2 gap-20 items-center">

                    <AnimSection anim="anim-fade-left" className="relative">
                        <div className="absolute top-10 right-1 bg-primary p-5 text-center shadow-xl"
                            style={{ animation: "pulse-badge 3s ease-in-out infinite" }}>
                            <p className="font-serif text-4xl font-medium leading-none text-bg-white">5★</p>
                            <p className="text-[9px] tracking-[0.3em] uppercase font-semibold mt-1 text-bg-white/80">Calificación</p>
                        </div>

                        <div className="flex items-center gap-4 mb-8">
                            <div className="w-12 h-px bg-primary" />
                            <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">Desde 2012</span>

                        </div>
                        <h2 className="font-serif text-4xl md:text-6xl text-text-main font-medium leading-tight mb-8">
                            No es solo<br />un hotel.<br />
                            <span className="text-primary italic">Es un estado de ánimo.</span>
                        </h2>
                        <p className="text-text-sub text-base leading-relaxed mb-5">
                            ALTI Hotel nació de la convicción de que el lujo verdadero no se mide en metros cuadrados,
                            sino en la calidad de los momentos que vivimos.
                        </p>
                        <p className="text-text-sub text-base leading-relaxed mb-10">
                            Ubicados en el corazón de Santo Domingo, somos el punto de encuentro entre la herencia
                            cultural dominicana y el estándar de servicio más exigente del mundo.
                        </p>
                        <div className="flex gap-4 flex-wrap">
                            <Link to="/about" className="btn-primary px-8 py-3.5">Nuestra historia</Link>
                            <Link to="/contact" className="btn-outline px-8 py-3.5">Contáctenos</Link>
                        </div>
                    </AnimSection>

                    <AnimSection anim="anim-fade-right" className="relative h-[520px]">

                        <img
                            src="https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=800&q=80"
                            alt="Suite"
                            className="absolute top-0 right-0 w-3/4 h-80 object-cover shadow-2xl"
                            style={{ transition: "transform 0.6s ease", cursor: "default" }}
                            onMouseEnter={e => (e.currentTarget.style.transform = "scale(1.03)")}
                            onMouseLeave={e => (e.currentTarget.style.transform = "scale(1)")}
                        />
                        <img
                            src="https://images.unsplash.com/photo-1584132967334-10e028bd69f7?w=800&q=80"
                            alt="Spa"
                            className="absolute bottom-0 left-0 w-2/3 h-64 object-cover shadow-2xl"
                            style={{ transition: "transform 0.6s ease", cursor: "default" }}
                            onMouseEnter={e => (e.currentTarget.style.transform = "scale(1.03)")}
                            onMouseLeave={e => (e.currentTarget.style.transform = "scale(1)")}
                        />

                    </AnimSection>
                </div>
            </section>

            {/* ═══ DIVIDER ══════════════════════════════════ */}
            <SectionDivider from="var(--bg-100)" to="var(--bg-200)" flip />

            {/* ═══ 4. HABITACIONES ══════════════════════════ */}
            <section className="py-32 px-6 bg-bg-card">
                <div className="max-w-7xl mx-auto">
                    <div className="flex flex-col md:flex-row md:items-end justify-between mb-16 gap-6">
                        <AnimSection anim="anim-fade-left">
                            <div className="flex items-center gap-4 mb-4">
                                <div className="w-8 h-px bg-primary" />
                                <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">Alojamiento</span>
                            </div>
                            <h2 className="font-serif text-4xl md:text-5xl text-text-main font-medium">
                                Nuestras habitaciones<br />más solicitadas
                            </h2>
                        </AnimSection>
                        <AnimSection anim="anim-fade-right">
                            <Link to="/rooms" className="btn-outline px-8 py-3.5 self-start whitespace-nowrap">Ver todas →</Link>
                        </AnimSection>
                    </div>

                    <StaggerList
                        items={rooms}
                        className="grid grid-cols-1 md:grid-cols-3 gap-6"
                        renderItem={(room) => (
                            <div className="group relative overflow-hidden bg-bg-base hover:shadow-2xl transition-shadow duration-500">
                                <div className="relative h-72 overflow-hidden">
                                    <img src={room.img} alt={room.type}
                                        className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-110" />
                                    <div className="absolute inset-0 bg-gradient-to-t from-black/80 via-black/20 to-transparent" />
                                    <span className="absolute top-4 left-4 bg-primary text-white text-[9px] tracking-[0.2em] uppercase px-3 py-1.5 font-semibold">
                                        {room.tag}
                                    </span>
                                    {/* Hover overlay */}
                                    <div className="absolute inset-0 bg-primary/0 group-hover:bg-primary/10 transition-all duration-500" />
                                </div>
                                <div className="p-6">
                                    <div className="flex justify-between items-start mb-3">
                                        <h3 className="font-serif text-xl text-text-main font-medium">{room.type}</h3>
                                        <div className="text-right">
                                            <p className="text-primary font-bold text-lg">${room.price}</p>
                                            <p className="text-text-sub text-[10px] tracking-widest uppercase font-medium">/noche</p>
                                        </div>
                                    </div>
                                    <p className="text-text-sub text-sm mb-5 leading-relaxed">{room.desc}</p>
                                    <Link to="/rooms"
                                        className="text-[10px] tracking-[0.25em] uppercase font-semibold text-primary border-b border-primary/30 pb-0.5 hover:border-primary transition-colors duration-200">
                                        Reservar ahora →
                                    </Link>
                                </div>
                            </div>
                        )}
                    />
                </div>
            </section>

            {/* ═══ DIVIDER ══════════════════════════════════ */}
            <SectionDivider from="var(--bg-200)" to="var(--bg-100)" />

            {/* ═══ 5. GALERÍA ═══════════════════════════════ */}
            <section className="py-32 px-6 bg-bg-base">
                <div className="max-w-7xl mx-auto">

                    <AnimSection className="text-center mb-16">
                        <div className="flex items-center justify-center gap-4 mb-4">
                            <div className="w-8 h-px bg-primary" />
                            <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">Galería</span>
                            <div className="w-8 h-px bg-primary" />
                        </div>
                        <h2 className="font-serif text-4xl md:text-5xl text-text-main font-medium">
                            Un vistazo a nuestro mundo
                        </h2>
                        <p className="text-text-sub text-sm mt-4 max-w-xl mx-auto">
                            Cada imagen cuenta una historia. Descubra los espacios, sabores y momentos únicos.
                        </p>
                    </AnimSection>

                    <AnimSection anim="anim-scale">
                        <div className="grid grid-cols-3 grid-rows-3 gap-3 h-[600px]">
                            {gallery.map((item, i) => (
                                <div
                                    key={i}
                                    onClick={() => setLightbox(i)}
                                    className={`relative overflow-hidden cursor-zoom-in group ${item.span}`}
                                >
                                    <img src={item.img} alt={item.label}
                                        className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-110" />
                                    <div className="absolute inset-0 bg-black/0 group-hover:bg-black/45 transition-all duration-400" />
                                    <div className="absolute inset-0 flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity duration-300">
                                        <span className="text-white text-3xl">⊕</span>
                                    </div>
                                    <div className="absolute bottom-0 left-0 right-0 p-4 translate-y-full group-hover:translate-y-0 transition-transform duration-400">
                                        <span className="text-white text-xs tracking-[0.2em] uppercase font-semibold bg-primary px-3 py-1.5">
                                            {item.label}
                                        </span>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </AnimSection>
                </div>
            </section>

            {/* Lightbox */}
            {lightbox !== null && (
                <div
                    className="fixed inset-0 z-50 bg-black/92 flex items-center justify-center p-6"
                    style={{ animation: "fadeInUp 0.3s ease" }}
                    onClick={() => setLightbox(null)}
                >
                    <button
                        className="absolute top-6 right-6 text-white/50 hover:text-white text-3xl transition-colors z-10"
                        onClick={() => setLightbox(null)}
                    >
                        ✕
                    </button>
                    {/* Prev */}
                    <button
                        className="absolute left-6 text-white/50 hover:text-white text-4xl transition-colors z-10"
                        onClick={e => { e.stopPropagation(); setLightbox(p => p === null ? 0 : (p - 1 + gallery.length) % gallery.length); }}
                    >
                        ‹
                    </button>
                    <img
                        src={gallery[lightbox].img}
                        alt={gallery[lightbox].label}
                        className="max-h-[85vh] max-w-full object-contain shadow-2xl"
                        style={{ animation: "fadeInUp 0.3s ease" }}
                        onClick={e => e.stopPropagation()}
                    />
                    {/* Next */}
                    <button
                        className="absolute right-6 text-white/50 hover:text-white text-4xl transition-colors z-10"
                        onClick={e => { e.stopPropagation(); setLightbox(p => p === null ? 0 : (p + 1) % gallery.length); }}
                    >
                        ›
                    </button>
                    <div className="absolute bottom-8 left-1/2 -translate-x-1/2 text-center">
                        <p className="text-white/60 text-xs tracking-widest uppercase font-semibold mb-3">
                            {gallery[lightbox].label}
                        </p>
                        <div className="flex gap-2 justify-center">
                            {gallery.map((_, i) => (
                                <button key={i}
                                    onClick={e => { e.stopPropagation(); setLightbox(i); }}
                                    className={`rounded-full transition-all duration-300 ${i === lightbox ? "w-6 h-1.5 bg-primary" : "w-1.5 h-1.5 bg-white/30"}`}
                                />
                            ))}
                        </div>
                    </div>
                </div>
            )}

            {/* ═══ DIVIDER ══════════════════════════════════ */}
            <SectionDivider from="var(--bg-100)" to="var(--bg-200)" flip />

            {/* ═══ 6. AMENIDADES ════════════════════════════ */}
            <section className="py-32 px-6 bg-bg-card">
                <div className="max-w-7xl mx-auto">
                    <div className="flex flex-col md:flex-row gap-16 items-start">

                        <AnimSection anim="anim-fade-left" className="md:w-1/3 md:sticky md:top-24">
                            <div className="flex items-center gap-4 mb-6">
                                <div className="w-8 h-px bg-primary" />
                                <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">Instalaciones</span>
                            </div>
                            <h2 className="font-serif text-4xl md:text-5xl text-text-main font-medium leading-tight mb-6">
                                Todo lo que<br />necesita,<br />
                                <span className="text-primary italic">y más.</span>
                            </h2>
                            <p className="text-text-sub text-sm leading-relaxed mb-8">
                                Nuestras instalaciones han sido diseñadas para que no tenga que salir del hotel a menos que lo desee.
                            </p>
                            <Link to="/services" className="btn-primary px-8 py-3.5">Ver todos los servicios</Link>
                        </AnimSection>

                        <StaggerList
                            items={amenities}
                            className="md:w-2/3 grid grid-cols-1 sm:grid-cols-2 gap-px bg-bg-base"
                            renderItem={(a) => (
                                <div className="bg-bg-card p-8 hover:bg-bg-base transition-all duration-300 group cursor-default">
                                    <span className="text-primary text-2xl block mb-4 group-hover:scale-125 group-hover:rotate-12 transition-transform duration-400 origin-left">
                                        {a.icon}
                                    </span>
                                    <h3 className="font-serif text-xl text-text-main font-medium mb-2">{a.title}</h3>
                                    <p className="text-text-sub text-sm leading-relaxed">{a.desc}</p>
                                </div>
                            )}
                        />
                    </div>
                </div>
            </section>

            {/* ═══ DIVIDER ══════════════════════════════════ */}
            <SectionDivider from="var(--bg-200)" to="var(--bg-100)" />

            {/* ═══ 7. EXPERIENCES ═══════════════════════════ */}
            <section className="py-32 px-6 bg-bg-base">
                <div className="max-w-7xl mx-auto">

                    <AnimSection className="text-center mb-16">
                        <div className="flex items-center justify-center gap-4 mb-4">
                            <div className="w-8 h-px bg-primary" />
                            <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">Experiencias</span>
                            <div className="w-8 h-px bg-primary" />
                        </div>
                        <h2 className="font-serif text-4xl md:text-5xl text-text-main font-medium">
                            Momentos que recordará<br />para siempre
                        </h2>
                    </AnimSection>

                    <StaggerList
                        items={experiences}
                        className="grid grid-cols-1 md:grid-cols-3 gap-6"
                        renderItem={(exp) => (
                            <div className="group relative overflow-hidden bg-bg-card hover:shadow-2xl transition-shadow duration-500">
                                <div className="relative h-60 overflow-hidden">
                                    <img src={exp.img} alt={exp.title}
                                        className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-110" />
                                    <div className="absolute inset-0 bg-gradient-to-t from-black/60 to-transparent" />
                                    <div className="absolute bottom-4 left-4">
                                        <span className="bg-primary text-white text-[10px] tracking-widest uppercase px-3 py-1.5 font-semibold">
                                            {exp.price}
                                        </span>
                                    </div>
                                </div>
                                <div className="p-6">
                                    <h3 className="font-serif text-xl text-text-main font-medium mb-2">{exp.title}</h3>
                                    <p className="text-text-sub text-sm leading-relaxed mb-4">{exp.desc}</p>
                                    <Link to="/services"
                                        className="text-[10px] tracking-[0.25em] uppercase font-semibold text-primary border-b border-primary/30 pb-0.5 hover:border-primary transition-colors">
                                        Reservar →
                                    </Link>
                                </div>
                            </div>
                        )}
                    />
                </div>
            </section>

            {/* ═══ DIVIDER ══════════════════════════════════ */}
            <SectionDivider from="var(--bg-100)" to="var(--bg-200)" flip />

            {/* ═══ 8. LOCATION ══════════════════════════════ */}
            <section className="py-32 px-6 bg-bg-card">
                <div className="max-w-7xl mx-auto grid grid-cols-1 lg:grid-cols-2 gap-16 items-center">

                    <AnimSection anim="anim-scale">
                        <div className="relative h-96 overflow-hidden group">
                            <img
                                src="https://images.unsplash.com/photo-1524813686514-a57563d77965?w=1200&q=80"
                                alt="Ubicación"
                                className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-105"
                            />
                            <div className="absolute inset-0 bg-primary/20" />
                            <div className="absolute inset-0 flex items-center justify-center">
                                <div className="text-center text-white" style={{ animation: "pulse-badge 3s ease-in-out infinite" }}>
                                    <div className="w-14 h-14 rounded-full bg-primary flex items-center justify-center mx-auto mb-4 shadow-lg shadow-primary/40">
                                        <span className="text-white text-2xl">◈</span>
                                    </div>
                                    <p className="font-serif text-2xl font-medium">Santo Domingo</p>
                                    <p className="text-white/70 text-sm tracking-widest uppercase font-medium mt-1">República Dominicana</p>
                                </div>
                            </div>
                        </div>
                    </AnimSection>

                    <AnimSection anim="anim-fade-right">
                        <div className="flex items-center gap-4 mb-6">
                            <div className="w-8 h-px bg-primary" />
                            <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">Ubicación</span>
                        </div>
                        <h2 className="font-serif text-4xl md:text-5xl text-text-main font-medium leading-tight mb-6">
                            En el corazón<br />de la ciudad
                        </h2>
                        <p className="text-text-sub text-sm leading-relaxed mb-8">
                            A minutos de los principales atractivos culturales, centros financieros y playas de Santo Domingo.
                        </p>
                        <div className="space-y-5 mb-10">
                            {[
                                { icon: "◈", label: "Dirección", value: "Av. Libertad 100, Santo Domingo, RD" },
                                { icon: "◈", label: "Teléfono", value: "+1 (809) 000-0000" },
                                { icon: "◈", label: "Email", value: "info@altihotel.com" },
                                { icon: "◈", label: "Aeropuerto (SDQ)", value: "25 min en traslado privado" },
                                { icon: "◈", label: "Zona Colonial", value: "8 min a pie" },
                            ].map(item => (
                                <div key={item.label} className="flex gap-4 items-center group cursor-default">
                                    <span className="text-primary text-base flex-shrink-0 group-hover:scale-125 transition-transform duration-300">{item.icon}</span>
                                    <div className="flex gap-2 flex-wrap">
                                        <span className="text-[10px] tracking-[0.2em] uppercase text-text-sub font-semibold">{item.label}:</span>
                                        <span className="text-text-main text-sm font-medium">{item.value}</span>
                                    </div>
                                </div>
                            ))}
                        </div>
                        <Link to="/contact" className="btn-primary px-8 py-3.5">Cómo llegar</Link>
                    </AnimSection>
                </div>
            </section>

            {/* ═══ DIVIDER ══════════════════════════════════ */}
            <SectionDivider from="var(--bg-200)" to="var(--bg-100)" />

            {/* ═══ 9. REVIEWS ═══════════════════════════════ */}
            <section className="py-32 px-6 bg-bg-base overflow-hidden">
                <div className="max-w-4xl mx-auto">

                    <AnimSection className="text-center mb-20">
                        <div className="flex items-center justify-center gap-4 mb-4">
                            <div className="w-8 h-px bg-primary" />
                            <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">Testimonios</span>
                            <div className="w-8 h-px bg-primary" />
                        </div>
                        <h2 className="font-serif text-4xl md:text-5xl text-text-main font-medium">
                            Lo que dicen<br />nuestros huéspedes
                        </h2>
                    </AnimSection>

                    <div className="relative min-h-[240px]">
                        {reviews.map((r, i) => (
                            <div key={i} className={`absolute inset-0 transition-all duration-700 ${i === review ? "opacity-100 translate-y-0" : "opacity-0 translate-y-8 pointer-events-none"
                                }`}>
                                <div className="flex justify-center gap-1 mb-8">
                                    {Array.from({ length: r.rating }).map((_, j) => (
                                        <span key={j} className="text-primary text-2xl" style={{ animationDelay: `${j * 0.1}s` }}>★</span>
                                    ))}
                                </div>
                                <p className="font-serif text-xl md:text-2xl text-text-main font-medium italic leading-relaxed mb-10 text-center max-w-2xl mx-auto">
                                    "{r.text}"
                                </p>
                                <div className="text-center">
                                    <div className="w-12 h-12 rounded-full bg-primary/20 border-2 border-primary/30 flex items-center justify-center mx-auto mb-3">
                                        <span className="text-primary font-serif text-xl font-medium">{r.name.charAt(0)}</span>
                                    </div>
                                    <p className="text-text-main font-semibold text-sm tracking-widest">{r.name}</p>
                                    <p className="text-text-sub text-xs tracking-widest uppercase font-medium mt-1">{r.country}</p>
                                </div>
                            </div>
                        ))}
                    </div>

                    <div className="flex justify-center gap-3 mt-20">
                        {reviews.map((_, i) => (
                            <button key={i} onClick={() => setReview(i)}
                                className={`rounded-full transition-all duration-300 ${i === review ? "w-8 h-1.5 bg-primary" : "w-1.5 h-1.5 bg-primary/30 hover:bg-primary/60"
                                    }`} />
                        ))}
                    </div>
                </div>
            </section>

            {/* ═══ 10. CTA FINAL ════════════════════════════ */}
            <section className="relative py-44 px-6 overflow-hidden">
                <img
                    src="https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?w=1800&q=80"
                    alt="CTA"
                    className="absolute inset-0 w-full h-full object-cover"
                />
                <div className="absolute inset-0 bg-primary/85" />

                {/* Animated rings */}
                <div className="absolute inset-0 flex items-center justify-center pointer-events-none overflow-hidden">
                    {[1, 2, 3].map(i => (
                        <div key={i}
                            className="absolute rounded-full border border-white/10"
                            style={{
                                width: `${i * 280}px`,
                                height: `${i * 280}px`,
                                animation: `ripple ${3 + i}s ease-out infinite`,
                                animationDelay: `${i * 0.8}s`,
                            }}
                        />
                    ))}
                </div>

                <AnimSection className="relative text-center text-white max-w-3xl mx-auto">
                    <p className="text-white/70 text-[10px] tracking-[0.5em] uppercase font-semibold mb-6">Reserve su estadía</p>
                    <h2 className="font-serif text-5xl md:text-7xl font-medium leading-tight mb-6">
                        Su experiencia<br />perfecta comienza<br />
                        <span className="italic text-white/80">aquí.</span>
                    </h2>
                    <p className="text-white/60 text-sm md:text-base mb-12 max-w-lg mx-auto font-medium">
                        Disponibilidad limitada. Reserve con antelación para garantizar su suite preferida.
                    </p>
                    <div className="flex flex-col sm:flex-row gap-4 justify-center">
                        <Link to="/rooms" className="btn-primary px-10 py-4 text-xs">
                            Verificar disponibilidad
                        </Link>
                        <Link to="/contact"
                            className="border border-white/40 text-white text-xs tracking-[0.2em] uppercase font-semibold px-10 py-4 hover:bg-white/10 transition-colors duration-300">
                            Hablar con concierge
                        </Link>
                    </div>
                </AnimSection>
            </section>

            {/* ═══ 11. FOOTER ═══════════════════════════════ */}
            <footer className="bg-bg-card pt-20 pb-10 px-6">
                <div className="max-w-7xl mx-auto">
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-12 mb-16">
                        <div className="lg:col-span-2">
                            <p className="font-serif text-text-main text-4xl font-medium tracking-[0.08em]">ALTI</p>
                            <p className="text-primary text-[9px] tracking-[0.5em] uppercase font-semibold mt-1 mb-6">Hotel & Resort</p>
                            <p className="text-text-sub text-sm leading-relaxed max-w-xs mb-6">
                                Donde cada momento se convierte en un recuerdo imborrable. Lujo, confort y servicio excepcional desde 2012.
                            </p>
                            <div className="flex gap-3">
                                {["ig", "fb", "tw", "yt"].map(s => (
                                    <div key={s}
                                        className="w-9 h-9 border border-bg-base flex items-center justify-center text-text-sub hover:border-primary hover:text-primary transition-all duration-300 cursor-pointer hover:scale-110">
                                        <span className="text-[10px] uppercase font-bold">{s}</span>
                                    </div>
                                ))}
                            </div>
                        </div>
                        <div>
                            <p className="text-primary text-[10px] tracking-[0.4em] uppercase font-bold mb-6">Navegación</p>
                            <div className="space-y-3">
                                {[{ to: "/", l: "Inicio" }, { to: "/rooms", l: "Habitaciones" }, { to: "/services", l: "Servicios" }, { to: "/about", l: "Nosotros" }, { to: "/contact", l: "Contacto" }].map(i => (
                                    <Link key={i.to} to={i.to}
                                        className="block text-text-sub hover:text-primary text-sm font-medium tracking-wide transition-colors duration-200 hover:translate-x-1 transform">
                                        {i.l}
                                    </Link>
                                ))}
                            </div>
                        </div>
                        <div>
                            <p className="text-primary text-[10px] tracking-[0.4em] uppercase font-bold mb-6">Contacto</p>
                            <div className="space-y-3 text-text-sub text-sm font-medium">
                                <p>info@altihotel.com</p>
                                <p>+1 (809) 000-0000</p>
                                <p className="leading-relaxed">Av. Libertad 100<br />Santo Domingo, RD</p>
                                <p className="text-text-sub/60 text-xs font-medium mt-2">
                                    Check-in: 3:00 PM<br />Check-out: 12:00 PM
                                </p>
                            </div>
                        </div>
                    </div>
                    <div className="border-t border-bg-base pt-8 flex flex-col md:flex-row justify-between items-center gap-4">
                        <p className="text-text-sub text-[10px] tracking-widest uppercase font-medium">
                            © 2026 ALTI Hotel. Todos los derechos reservados.
                        </p>
                        <p className="text-text-sub text-[10px] tracking-widest uppercase font-medium">
                            Programación 2 — Enmanuel Chavez & Delis De la Cruz
                        </p>
                    </div>
                </div>
            </footer>

            {/* ═══ KEYFRAMES ════════════════════════════════ */}
            <style>{`
        @keyframes fadeInUp {
          from { opacity: 0; transform: translateY(28px); }
          to   { opacity: 1; transform: translateY(0);    }
        }
        @keyframes marquee {
          from { transform: translateX(0);       }
          to   { transform: translateX(-33.33%); }
        }
        @keyframes scrollDrop {
          0%   { top: 0;    opacity: 1; }
          80%  { top: 55%;  opacity: 1; }
          100% { top: 100%; opacity: 0; }
        }
        @keyframes pulse-badge {
          0%, 100% { transform: scale(1);    box-shadow: 0 0 0 0 rgba(var(--primary-rgb), 0); }
          50%       { transform: scale(1.04); box-shadow: 0 0 20px 4px rgba(139,95,191,0.3);   }
        }
        @keyframes ripple {
          0%   { transform: scale(0.6); opacity: 0.5; }
          100% { transform: scale(2);   opacity: 0;   }
        }
      `}</style>
        </div>
    );
}