import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import AdditionalServiceService from "../../services/AdditionalServiceService";
import type { AdditionalServiceDto } from "../../models/AdditionalService";
import { useSEO } from "../../hooks/useSEO";

export default function ServicesPage() {
  useSEO({ title: "Servicios", description: "Spa, alta cocina, traslados y experiencias exclusivas en ALTI Hotel." });

  const [apiServices, setApiServices] = useState<AdditionalServiceDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    AdditionalServiceService.getAllActive()
      .then(setApiServices)
      .catch(() => setApiServices([]))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="min-h-screen bg-bg-base pt-20">

      <div className="relative h-56 md:h-80 overflow-hidden">
        <img src="https://images.unsplash.com/photo-1540555700478-4be289fbecef?w=1800&q=80" alt="Services" className="w-full h-full object-cover" />
        <div className="absolute inset-0 bg-black/55" />
        <div className="absolute inset-0 flex flex-col items-center justify-center text-center text-white">
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase mb-3">ALTI Hotel</span>
          <h1 className="font-serif text-4xl md:text-6xl">Servicios & Experiencias</h1>
        </div>
      </div>

      <div className="max-w-2xl mx-auto text-center py-16 px-6">
        <p className="text-text-sub text-sm leading-relaxed">
          Cada servicio ha sido cuidadosamente diseñado para elevar su estadía.
          Nuestro equipo está disponible para personalizar cualquier solicitud.
        </p>
      </div>

      <div className="max-w-7xl mx-auto px-6 pb-24">

        {loading && (
          <div className="text-center py-10 mb-10">
            <div className="w-6 h-6 border-2 border-primary border-t-transparent rounded-full animate-spin mx-auto" />
          </div>
        )}

        {!loading && apiServices.length === 0 && (
          <div className="text-center py-16">
            <span className="text-primary text-3xl block mb-4">◈</span>
            <p className="font-serif text-2xl text-text-sub/50 mb-2">Sin servicios disponibles</p>
            <p className="text-text-sub text-sm tracking-widest uppercase">Próximamente agregaremos nuevas experiencias</p>
          </div>
        )}

        {!loading && apiServices.length > 0 && (
          <div className="mb-20">
            <div className="flex items-center gap-6 mb-10">
              <div className="h-px flex-1 bg-bg-card" />
              <span className="text-primary text-[10px] tracking-[0.4em] uppercase whitespace-nowrap">Servicios disponibles</span>
              <div className="h-px flex-1 bg-bg-card" />
            </div>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              {apiServices.map(s => (
                <div key={s.id} className="bg-bg-white border border-bg-card p-8 hover:shadow-lg transition-all duration-300 group">
                  <span className="text-primary text-xl block mb-4 group-hover:scale-110 transition-transform duration-300">◈</span>
                  <div className="flex justify-between items-start mb-3">
                    <h3 className="font-serif text-xl text-text-main">{s.name}</h3>
                    <span className="text-primary font-sans text-sm font-medium ml-4 whitespace-nowrap">${s.price}</span>
                  </div>
                  {s.description && <p className="text-text-sub text-xs leading-relaxed">{s.description}</p>}
                </div>
              ))}
            </div>
          </div>
        )}

        <div className="text-center py-16 border-t border-bg-card">
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase">¿Listo para reservar?</span>
          <h2 className="font-serif text-3xl md:text-4xl text-text-main mt-3 mb-6">Reserve su experiencia perfecta</h2>
          <Link to="/rooms" className="btn-primary px-12 py-4">Ver Habitaciones</Link>
        </div>
      </div>
    </div>
  );
}