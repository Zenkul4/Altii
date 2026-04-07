import { useState, useEffect, useRef, useCallback } from "react";
import { useSEO } from "../../hooks/useSEO";
import { useScrollAnimation } from "../../hooks/useScrollAnimation";
import SectionDivider from "../components/SectionDivider";

/* ── DATA ─────────────────────────────────────────────── */
type Category = "all" | "habitaciones" | "spa" | "restaurante" | "piscina" | "exterior";

interface GalleryItem {
  id:       number;
  img:      string;
  title:    string;
  category: Category;
  span?:    "wide" | "tall" | "normal";
}

const CATEGORIES: { value: Category; label: string; icon: string }[] = [
  { value: "all",          label: "Todo",         icon: "◈" },
  { value: "habitaciones", label: "Habitaciones", icon: "◇" },
  { value: "spa",          label: "Spa",          icon: "◉" },
  { value: "restaurante",  label: "Restaurante",  icon: "◎" },
  { value: "piscina",      label: "Piscina",      icon: "⊕" },
  { value: "exterior",     label: "Exterior",     icon: "◌" },
];

const IMAGES: GalleryItem[] = [
  // Habitaciones
  { id:1,  img:"https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=1200&q=90", title:"Suite Presidencial",    category:"habitaciones", span:"wide"   },
  { id:2,  img:"https://images.unsplash.com/photo-1618773928121-c32242e63f39?w=800&q=90",  title:"Suite Deluxe",          category:"habitaciones", span:"normal" },
  { id:3,  img:"https://images.unsplash.com/photo-1566665797739-1674de7a421a?w=800&q=90",  title:"Habitación Double",     category:"habitaciones", span:"normal" },
  { id:4,  img:"https://images.unsplash.com/photo-1578683010236-d716f9a3f461?w=800&q=90",  title:"Suite Familiar",        category:"habitaciones", span:"tall"   },
  { id:5,  img:"https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?w=1200&q=90", title:"Penthouse",             category:"habitaciones", span:"wide"   },
  { id:6,  img:"https://images.unsplash.com/photo-1560448204-603b3fc33ddc?w=800&q=90",     title:"Habitación Single",     category:"habitaciones", span:"normal" },
  // Spa
  { id:7,  img:"https://images.unsplash.com/photo-1584132967334-10e028bd69f7?w=1200&q=90", title:"Spa & Wellness",        category:"spa",          span:"wide"   },
  { id:8,  img:"https://images.unsplash.com/photo-1540555700478-4be289fbecef?w=800&q=90",  title:"Sala de Tratamientos",  category:"spa",          span:"normal" },
  { id:9,  img:"https://images.unsplash.com/photo-1544161515-4ab6ce6db874?w=800&q=90",     title:"Sauna Finlandesa",      category:"spa",          span:"tall"   },
  { id:10, img:"https://images.unsplash.com/photo-1552693673-1bf958298935?w=800&q=90",     title:"Aromaterapia",          category:"spa",          span:"normal" },
  // Restaurante
  { id:11, img:"https://images.unsplash.com/photo-1414235077428-338989a2e8c0?w=1200&q=90", title:"Restaurante Principal", category:"restaurante",  span:"wide"   },
  { id:12, img:"https://images.unsplash.com/photo-1555396273-367ea4eb4db5?w=800&q=90",     title:"Cocina de Autor",       category:"restaurante",  span:"normal" },
  { id:13, img:"https://images.unsplash.com/photo-1537047902294-62a40c20a6ae?w=800&q=90",  title:"Bar & Lounge",          category:"restaurante",  span:"tall"   },
  { id:14, img:"https://images.unsplash.com/photo-1504674900247-0877df9cc836?w=800&q=90",  title:"Alta Cocina",           category:"restaurante",  span:"normal" },
  // Piscina
  { id:15, img:"https://images.unsplash.com/photo-1563911302283-d2bc129e7570?w=1200&q=90", title:"Piscina Infinita",      category:"piscina",      span:"wide"   },
  { id:16, img:"https://images.unsplash.com/photo-1559339352-11d035aa65de?w=800&q=90", title:"Pool Bar", category:"piscina", span:"normal" },
  { id:17, img:"https://images.unsplash.com/photo-1519046904884-53103b34b206?w=800&q=90",  title:"Zona de Relax",         category:"piscina",      span:"tall"   },
  { id:18, img:"https://images.unsplash.com/photo-1445019980597-93fa8acb246c?w=800&q=90",  title:"Piscina Termal",        category:"piscina",      span:"normal" },
  // Exterior
  { id:19, img:"https://images.unsplash.com/photo-1566073771259-6a8506099945?w=1200&q=90", title:"Vista Exterior",        category:"exterior",     span:"wide"   },
  { id:20, img:"https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?w=800&q=90",     title:"Jardines",              category:"exterior",     span:"normal" },
  { id:21, img:"https://images.unsplash.com/photo-1524813686514-a57563d77965?w=800&q=90",  title:"Terraza Principal",     category:"exterior",     span:"tall"   },
  { id:22, img:"https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=1200&q=90",    title:"Lobby & Entrada",       category:"exterior",     span:"wide"   },
];

/* ── Scroll animation wrapper ─────────────────────────── */
function AnimSection({ children, className="", anim="anim-fade-up" }: {
  children: React.ReactNode; className?: string; anim?: string;
}) {
  const { ref, visible } = useScrollAnimation();
  return (
    <div ref={ref} className={`${anim} ${visible?"visible":""} ${className}`}>
      {children}
    </div>
  );
}

/* ── Component ────────────────────────────────────────── */
export default function GalleryPage() {
  useSEO({ title:"Galería", description:"Explore los espacios, sabores y momentos únicos de ALTI Hotel." });

  const [active,   setActive]   = useState<Category>("all");
  const [lightbox, setLightbox] = useState<number | null>(null);
  const [filtered, setFiltered] = useState<GalleryItem[]>(IMAGES);
  const [animating,setAnimating]= useState(false);
  const carouselRef = useRef<HTMLDivElement>(null);
  const [carouselIdx, setCarouselIdx] = useState(0);

  // Carousel featured images
  const featured = IMAGES.filter(i => i.span === "wide").slice(0, 5);

  // Filter with animation
  const applyFilter = (cat: Category) => {
    if (cat === active) return;
    setAnimating(true);
    setTimeout(() => {
      setActive(cat);
      setFiltered(cat === "all" ? IMAGES : IMAGES.filter(i => i.category === cat));
      setAnimating(false);
    }, 300);
  };

  // Keyboard navigation for lightbox
  const handleKey = useCallback((e: KeyboardEvent) => {
    if (lightbox === null) return;
    if (e.key === "ArrowRight") setLightbox(p => p === null ? 0 : (p + 1) % filtered.length);
    if (e.key === "ArrowLeft")  setLightbox(p => p === null ? 0 : (p - 1 + filtered.length) % filtered.length);
    if (e.key === "Escape")     setLightbox(null);
  }, [lightbox, filtered]);

  useEffect(() => {
    document.addEventListener("keydown", handleKey);
    return () => document.removeEventListener("keydown", handleKey);
  }, [handleKey]);

  // Carousel auto-advance
  useEffect(() => {
    const t = setInterval(() => setCarouselIdx(p => (p + 1) % featured.length), 5000);
    return () => clearInterval(t);
  }, []);

  // Masonry grid columns
  const col1 = filtered.filter((_, i) => i % 3 === 0);
  const col2 = filtered.filter((_, i) => i % 3 === 1);
  const col3 = filtered.filter((_, i) => i % 3 === 2);

  return (
    <div className="bg-bg-base">

      {/* ═══ HERO ════════════════════════════════════ */}
      <section className="relative h-72 md:h-96 overflow-hidden">
        <img
          src="https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=1800&q=80"
          alt="Galería ALTI Hotel"
          className="w-full h-full object-cover"
          style={{ transform:"scale(1.05)" }}
        />
        <div className="absolute inset-0 bg-gradient-to-b from-black/60 via-black/40 to-black/70" />
        <div
          className="absolute inset-0 flex flex-col items-center justify-center text-center text-white px-6"
          style={{ animation:"fadeInUp 0.9s ease both" }}
        >
          <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold mb-4">
            ALTI Hotel
          </span>
          <h1 className="font-serif text-4xl md:text-6xl font-medium mb-4">
            Galería
          </h1>
          <p className="text-white/60 text-sm tracking-widest max-w-md font-medium">
            Explore los espacios, sabores y momentos que hacen de ALTI un destino único
          </p>
        </div>
        {/* Breadcrumb */}
        <div className="absolute bottom-6 left-6 flex items-center gap-2 text-white/40 text-[10px] tracking-widest uppercase font-medium">
          <a href="/" className="hover:text-white/70 transition-colors">Inicio</a>
          <span>›</span>
          <span className="text-primary">Galería</span>
        </div>
      </section>

      <SectionDivider from="transparent" to="var(--bg-100)" />

      {/* ═══ FEATURED CAROUSEL ════════════════════════ */}
      <section className="py-20 px-6 bg-bg-base">
        <div className="max-w-7xl mx-auto">

          <AnimSection className="flex items-end justify-between mb-10 gap-6 flex-wrap">
            <div>
              <div className="flex items-center gap-4 mb-3">
                <div className="w-8 h-px bg-primary" />
                <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">
                  Destacados
                </span>
              </div>
              <h2 className="font-serif text-3xl md:text-4xl text-text-main font-medium">
                Los más memorables
              </h2>
            </div>
            {/* Carousel dots */}
            <div className="flex gap-2">
              {featured.map((_, i) => (
                <button
                  key={i}
                  onClick={() => setCarouselIdx(i)}
                  className={`rounded-full transition-all duration-300 ${
                    i === carouselIdx
                      ? "w-8 h-1.5 bg-primary"
                      : "w-1.5 h-1.5 bg-primary/30 hover:bg-primary/60"
                  }`}
                />
              ))}
            </div>
          </AnimSection>

          {/* Carousel track */}
          <AnimSection anim="anim-scale" className="relative overflow-hidden">
            <div
              ref={carouselRef}
              className="flex transition-transform duration-700 ease-in-out"
              style={{ transform:`translateX(-${carouselIdx * 100}%)` }}
            >
              {featured.map((item, i) => (
                <div
                  key={item.id}
                  className="relative flex-shrink-0 w-full h-[420px] md:h-[520px] cursor-zoom-in group overflow-hidden"
                  onClick={() => {
                    const idx = filtered.findIndex(f => f.id === item.id);
                    if (idx !== -1) setLightbox(idx);
                    else { setActive("all"); setFiltered(IMAGES); setLightbox(IMAGES.findIndex(f => f.id === item.id)); }
                  }}
                >
                  <img
                    src={item.img}
                    alt={item.title}
                    className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-105"
                  />
                  <div className="absolute inset-0 bg-gradient-to-t from-black/70 via-transparent to-transparent" />

                  {/* Info overlay */}
                  <div className="absolute bottom-0 left-0 right-0 p-8 flex justify-between items-end">
                    <div>
                      <span className="bg-primary text-white text-[9px] tracking-[0.2em] uppercase px-3 py-1.5 font-semibold block mb-3 w-fit">
                        {CATEGORIES.find(c => c.value === item.category)?.label}
                      </span>
                      <h3 className="font-serif text-2xl md:text-3xl text-white font-medium">
                        {item.title}
                      </h3>
                    </div>
                    <div className="text-white/50 text-sm font-medium">
                      {i + 1} / {featured.length}
                    </div>
                  </div>

                  {/* Zoom icon */}
                  <div className="absolute top-6 right-6 w-10 h-10 rounded-full bg-black/30 backdrop-blur-sm flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity duration-300">
                    <span className="text-white text-lg">⊕</span>
                  </div>
                </div>
              ))}
            </div>

            {/* Prev/Next */}
            <button
              onClick={() => setCarouselIdx(p => (p - 1 + featured.length) % featured.length)}
              className="absolute left-4 top-1/2 -translate-y-1/2 w-12 h-12 bg-black/30 backdrop-blur-sm text-white text-2xl flex items-center justify-center hover:bg-primary transition-all duration-300"
            >
              ‹
            </button>
            <button
              onClick={() => setCarouselIdx(p => (p + 1) % featured.length)}
              className="absolute right-4 top-1/2 -translate-y-1/2 w-12 h-12 bg-black/30 backdrop-blur-sm text-white text-2xl flex items-center justify-center hover:bg-primary transition-all duration-300"
            >
              ›
            </button>
          </AnimSection>
        </div>
      </section>

      <SectionDivider from="var(--bg-100)" to="var(--bg-200)" flip />

      {/* ═══ MASONRY GALLERY ══════════════════════════ */}
      <section className="py-20 px-6 bg-bg-card">
        <div className="max-w-7xl mx-auto">

          {/* Header + filters */}
          <AnimSection className="mb-12">
            <div className="flex flex-col md:flex-row md:items-end justify-between gap-6 mb-8">
              <div>
                <div className="flex items-center gap-4 mb-3">
                  <div className="w-8 h-px bg-primary" />
                  <span className="text-primary text-[10px] tracking-[0.5em] uppercase font-semibold">
                    Explorar
                  </span>
                </div>
                <h2 className="font-serif text-3xl md:text-4xl text-text-main font-medium">
                  Todos los espacios
                </h2>
              </div>
              <p className="text-text-sub text-sm font-medium">
                {filtered.length} imagen{filtered.length !== 1 ? "es" : ""}
              </p>
            </div>

            {/* Filter tabs */}
            <div className="flex flex-wrap gap-2">
              {CATEGORIES.map(cat => (
                <button
                  key={cat.value}
                  onClick={() => applyFilter(cat.value)}
                  className={`flex items-center gap-2 text-[10px] tracking-[0.2em] uppercase px-5 py-2.5 border font-semibold transition-all duration-300 ${
                    active === cat.value
                      ? "bg-primary border-primary text-white scale-105 shadow-lg shadow-primary/20"
                      : "border-bg-base text-text-sub hover:border-primary hover:text-primary"
                  }`}
                >
                  <span className={`text-xs ${active === cat.value ? "text-white" : "text-primary"}`}>
                    {cat.icon}
                  </span>
                  {cat.label}
                </button>
              ))}
            </div>
          </AnimSection>

          {/* Masonry grid — 3 columns */}
          <div
            className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 transition-opacity duration-300"
            style={{ opacity: animating ? 0 : 1 }}
          >
            {[col1, col2, col3].map((col, ci) => (
              <div key={ci} className="flex flex-col gap-4">
                {col.map((item, ii) => {
                  const height = item.span === "wide" ? "h-72"
                               : item.span === "tall" ? "h-96"
                               : "h-60";
                  const delay  = ["delay-100","delay-200","delay-300","delay-400","delay-500","delay-600","delay-700","delay-800"];
                  return (
                    <MasonryCard
                      key={item.id}
                      item={item}
                      height={height}
                      delay={delay[(ci * 3 + ii) % 8]}
                      onClick={() => setLightbox(filtered.indexOf(item))}
                    />
                  );
                })}
              </div>
            ))}
          </div>

          {/* Empty state */}
          {filtered.length === 0 && (
            <div className="text-center py-24">
              <p className="font-serif text-3xl text-text-sub/30 font-medium mb-2">Sin imágenes</p>
              <p className="text-text-sub text-sm font-medium">No hay imágenes en esta categoría</p>
            </div>
          )}
        </div>
      </section>

      <SectionDivider from="var(--bg-200)" to="var(--bg-100)" />

      {/* ═══ CTA ══════════════════════════════════════ */}
      <section className="relative py-28 px-6 overflow-hidden">
        <img
          src="https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?w=1800&q=80"
          alt="Reservar"
          className="absolute inset-0 w-full h-full object-cover"
        />
        <div className="absolute inset-0 bg-primary/80" />
        <div className="relative text-center text-white max-w-2xl mx-auto"
          style={{ animation:"fadeInUp 0.9s ease both" }}>
          <span className="text-white/60 text-[10px] tracking-[0.5em] uppercase font-semibold block mb-4">
            ¿Le gustó lo que vio?
          </span>
          <h2 className="font-serif text-4xl md:text-5xl font-medium mb-6 leading-tight">
            Vívalo en persona
          </h2>
          <p className="text-white/60 text-sm mb-10 max-w-md mx-auto font-medium">
            Las fotos no hacen justicia. Reserve su estadía y experimente ALTI Hotel con todos sus sentidos.
          </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <a href="/rooms"
              className="bg-white text-primary text-xs tracking-[0.2em] uppercase font-bold px-10 py-4 hover:bg-white/90 transition-colors duration-300">
              Reservar ahora
            </a>
            <a href="/contact"
              className="border border-white/40 text-white text-xs tracking-[0.2em] uppercase font-semibold px-10 py-4 hover:bg-white/10 transition-colors duration-300">
              Contactar concierge
            </a>
          </div>
        </div>
      </section>

      {/* ═══ LIGHTBOX ════════════════════════════════ */}
      {lightbox !== null && filtered[lightbox] && (
        <div
          className="fixed inset-0 z-50 flex items-center justify-center"
          style={{ background:"rgba(0,0,0,0.95)", animation:"fadeInUp 0.3s ease" }}
          onClick={() => setLightbox(null)}
        >
          {/* Close */}
          <button
            className="absolute top-6 right-6 text-white/50 hover:text-white text-3xl transition-colors z-10 w-10 h-10 flex items-center justify-center"
            onClick={() => setLightbox(null)}
          >
            ✕
          </button>

          {/* Prev */}
          <button
            className="absolute left-4 md:left-8 top-1/2 -translate-y-1/2 w-12 h-12 border border-white/20 text-white/60 hover:border-primary hover:text-primary text-2xl flex items-center justify-center transition-all duration-300 z-10"
            onClick={e => { e.stopPropagation(); setLightbox(p => p === null ? 0 : (p - 1 + filtered.length) % filtered.length); }}
          >
            ‹
          </button>

          {/* Image */}
          <div
            className="flex flex-col items-center px-16 max-w-5xl w-full"
            onClick={e => e.stopPropagation()}
          >
            <img
              key={lightbox}
              src={filtered[lightbox].img}
              alt={filtered[lightbox].title}
              className="max-h-[75vh] max-w-full object-contain shadow-2xl"
              style={{ animation:"fadeInUp 0.35s ease" }}
            />

            {/* Caption */}
            <div className="mt-6 text-center">
              <span className="bg-primary text-white text-[9px] tracking-[0.2em] uppercase px-3 py-1.5 font-semibold">
                {CATEGORIES.find(c => c.value === filtered[lightbox].category)?.label}
              </span>
              <p className="font-serif text-xl text-white font-medium mt-3">
                {filtered[lightbox].title}
              </p>
              <p className="text-white/30 text-xs font-medium mt-2">
                {lightbox + 1} / {filtered.length} · Usa ← → para navegar · ESC para cerrar
              </p>
            </div>

            {/* Thumbnail strip */}
            <div className="flex gap-2 mt-6 overflow-x-auto max-w-full pb-2">
              {filtered.map((item, i) => (
                <button
                  key={item.id}
                  onClick={() => setLightbox(i)}
                  className={`flex-shrink-0 w-14 h-10 overflow-hidden border-2 transition-all duration-200 ${
                    i === lightbox ? "border-primary" : "border-transparent opacity-50 hover:opacity-80"
                  }`}
                >
                  <img src={item.img} alt={item.title} className="w-full h-full object-cover" />
                </button>
              ))}
            </div>
          </div>

          {/* Next */}
          <button
            className="absolute right-4 md:right-8 top-1/2 -translate-y-1/2 w-12 h-12 border border-white/20 text-white/60 hover:border-primary hover:text-primary text-2xl flex items-center justify-center transition-all duration-300 z-10"
            onClick={e => { e.stopPropagation(); setLightbox(p => p === null ? 0 : (p + 1) % filtered.length); }}
          >
            ›
          </button>
        </div>
      )}

      <style>{`
        @keyframes fadeInUp {
          from { opacity:0; transform:translateY(20px); }
          to   { opacity:1; transform:translateY(0); }
        }
      `}</style>
    </div>
  );
}

/* ── Masonry Card ──────────────────────────────────────── */
function MasonryCard({ item, height, delay, onClick }: {
  item:    GalleryItem;
  height:  string;
  delay:   string;
  onClick: () => void;
}) {
  const { ref, visible } = useScrollAnimation(0.05);
  const [hovered, setHovered] = useState(false);

  return (
    <div
      ref={ref}
      className={`anim-fade-up ${delay} ${visible ? "visible" : ""}`}
    >
      <div
        className={`relative ${height} overflow-hidden cursor-zoom-in group`}
        onClick={onClick}
        onMouseEnter={() => setHovered(true)}
        onMouseLeave={() => setHovered(false)}
        style={{
          transform:  hovered ? "translateY(-4px)" : "translateY(0)",
          transition: "transform 0.4s ease, box-shadow 0.4s ease",
          boxShadow:  hovered ? "0 20px 40px rgba(0,0,0,0.2)" : "none",
        }}
      >
        <img
          src={item.img}
          alt={item.title}
          className="w-full h-full object-cover transition-transform duration-700 group-hover:scale-110"
        />

        {/* Overlay */}
        <div className="absolute inset-0 bg-black/0 group-hover:bg-black/40 transition-all duration-400" />

        {/* Category badge */}
        <div className="absolute top-3 left-3">
          <span className="bg-primary text-white text-[9px] tracking-[0.15em] uppercase px-2.5 py-1 font-semibold opacity-0 group-hover:opacity-100 transition-opacity duration-300">
            {CATEGORIES.find(c => c.value === item.category)?.label}
          </span>
        </div>

        {/* Zoom icon */}
        <div className="absolute top-3 right-3 w-8 h-8 rounded-full bg-black/40 backdrop-blur-sm flex items-center justify-center opacity-0 group-hover:opacity-100 transition-opacity duration-300">
          <span className="text-white text-sm">⊕</span>
        </div>

        {/* Title */}
        <div className="absolute bottom-0 left-0 right-0 p-4 translate-y-full group-hover:translate-y-0 transition-transform duration-400">
          <p className="font-serif text-lg text-white font-medium">{item.title}</p>
        </div>
      </div>
    </div>
  );
}