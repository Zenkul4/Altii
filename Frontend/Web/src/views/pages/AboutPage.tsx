import { Link } from "react-router-dom";
import { useSEO } from "../../hooks/useSEO";

const team = [
  { name:"Carlos Méndez",    role:"Director General",          initial:"CM" },
  { name:"Sofía Reyes",      role:"Directora de Hospitalidad", initial:"SR" },
  { name:"Marco Villanueva", role:"Chef Ejecutivo",            initial:"MV" },
  { name:"Elena Castillo",   role:"Directora de Spa",          initial:"EC" },
];
const values = [
  { icon:"◈", title:"Excelencia",    desc:"Cada detalle está diseñado para superar sus expectativas."      },
  { icon:"◈", title:"Autenticidad",  desc:"Experiencias genuinas que conectan con la cultura local."      },
  { icon:"◈", title:"Sostenibilidad",desc:"Prácticas responsables que preservan el entorno."               },
  { icon:"◈", title:"Discreción",    desc:"La privacidad de nuestros huéspedes es sagrada."               },
];
const milestones = [
  { year:"2012", event:"Fundación de ALTI Hotel con 20 habitaciones"         },
  { year:"2015", event:"Primera distinción Five Stars Alliance"              },
  { year:"2018", event:"Apertura del Spa & Wellness Center"                  },
  { year:"2020", event:"Reconocimiento como mejor hotel boutique del Caribe" },
  { year:"2023", event:"Expansión a 48 habitaciones y suites"                },
  { year:"2026", event:"Lanzamiento de la plataforma digital ALTI"           },
];

export default function AboutPage() {
  useSEO({ title:"Acerca de nosotros", description:"Conozca la historia, valores y equipo detrás de ALTI Hotel." });

  return (
    <div className="min-h-screen bg-bg-base pt-20">

      <div className="relative h-64 md:h-96 overflow-hidden">
        <img src="https://images.unsplash.com/photo-1566073771259-6a8506099945?w=1800&q=80" alt="ALTI Hotel" className="w-full h-full object-cover" />
        <div className="absolute inset-0 bg-black/55" />
        <div className="absolute inset-0 flex flex-col items-center justify-center text-white text-center px-6">
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase mb-3">Nuestra historia</span>
          <h1 className="font-serif text-4xl md:text-6xl">Acerca de ALTI Hotel</h1>
        </div>
      </div>

      {/* Intro */}
      <section className="max-w-4xl mx-auto px-6 py-20 text-center">
        <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Desde 2012</span>
        <h2 className="font-serif text-3xl md:text-5xl text-text-main mt-4 mb-8">
          Más que un hotel,<br />una filosofía de vida
        </h2>
        <p className="text-text-sub text-sm leading-relaxed max-w-2xl mx-auto mb-6">
          ALTI Hotel nació de un sueño sencillo: crear un espacio donde el lujo se sintiera natural,
          donde cada huésped fuera tratado como el invitado más importante del mundo.
        </p>
        <p className="text-text-sub text-sm leading-relaxed max-w-2xl mx-auto">
          Cada habitación, cada plato, cada sonrisa de nuestro equipo es una expresión de
          nuestra pasión por la hospitalidad auténtica.
        </p>
      </section>

      {/* Stats */}
      <section className="bg-bg-card py-12 px-6">
        <div className="max-w-5xl mx-auto grid grid-cols-2 md:grid-cols-4 gap-8 text-center">
          {[{num:"12+",label:"Años de experiencia"},{num:"48",label:"Habitaciones y suites"},{num:"5★",label:"Calificación promedio"},{num:"50k+",label:"Huéspedes satisfechos"}].map(stat=>(
            <div key={stat.label}>
              <p className="font-serif text-primary text-4xl">{stat.num}</p>
              <p className="text-text-sub text-[10px] tracking-[0.2em] uppercase mt-2">{stat.label}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Values */}
      <section className="py-20 px-6 bg-bg-base">
        <div className="max-w-6xl mx-auto">
          <div className="text-center mb-16">
            <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Lo que nos define</span>
            <h2 className="font-serif text-3xl md:text-4xl text-text-main mt-3">Nuestros valores</h2>
          </div>
          <div className="grid grid-cols-2 md:grid-cols-4 gap-px bg-bg-card">
            {values.map((v,i) => (
              <div key={i} className="bg-bg-base p-10 text-center hover:bg-bg-white transition-colors duration-300 group">
                <span className="text-primary text-2xl block mb-4 group-hover:scale-110 transition-transform duration-300">{v.icon}</span>
                <h3 className="font-serif text-xl text-text-main mb-3">{v.title}</h3>
                <p className="text-text-sub text-xs leading-relaxed">{v.desc}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Timeline */}
      <section className="py-20 px-6 bg-bg-card">
        <div className="max-w-3xl mx-auto">
          <div className="text-center mb-16">
            <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Nuestra trayectoria</span>
            <h2 className="font-serif text-3xl md:text-4xl text-text-main mt-3">Hitos de excelencia</h2>
          </div>
          <div className="relative">
            <div className="absolute left-1/2 -translate-x-px top-0 bottom-0 w-px bg-bg-base" />
            <div className="space-y-10">
              {milestones.map((m,i) => (
                <div key={i} className={`flex items-center gap-8 ${i%2===0?"flex-row":"flex-row-reverse"}`}>
                  <div className={`flex-1 ${i%2===0?"text-right":"text-left"}`}>
                    <p className="font-serif text-primary text-2xl mb-1">{m.year}</p>
                    <p className="text-text-sub text-sm">{m.event}</p>
                  </div>
                  <div className="w-3 h-3 rounded-full bg-primary flex-shrink-0 relative z-10" />
                  <div className="flex-1" />
                </div>
              ))}
            </div>
          </div>
        </div>
      </section>

      {/* Team */}
      <section className="py-20 px-6 bg-bg-base">
        <div className="max-w-5xl mx-auto">
          <div className="text-center mb-16">
            <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Quiénes somos</span>
            <h2 className="font-serif text-3xl md:text-4xl text-text-main mt-3">Nuestro equipo directivo</h2>
          </div>
          <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
            {team.map((member,i) => (
              <div key={i} className="text-center">
                <div className="w-20 h-20 rounded-full bg-primary/20 border border-primary/30 flex items-center justify-center mx-auto mb-4">
                  <span className="font-serif text-primary text-xl">{member.initial}</span>
                </div>
                <h3 className="font-serif text-text-main text-lg">{member.name}</h3>
                <p className="text-[10px] tracking-[0.2em] uppercase text-text-sub mt-1">{member.role}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CTA */}
      <section className="py-20 px-6 bg-bg-card text-center">
        <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Le esperamos</span>
        <h2 className="font-serif text-3xl md:text-4xl text-text-main mt-3 mb-6">Forme parte de nuestra historia</h2>
        <p className="text-text-sub text-sm mb-10 max-w-md mx-auto">
          Cada visita a ALTI Hotel es una nueva página en nuestra historia compartida.
        </p>
        <div className="flex gap-4 justify-center flex-wrap">
          <Link to="/rooms"   className="btn-primary px-10 py-4">Reservar ahora</Link>
          <Link to="/contact" className="btn-outline px-10 py-4">Contactarnos</Link>
        </div>
      </section>

      <footer className="bg-bg-card py-10 px-6 text-center border-t border-bg-base">
        <p className="text-text-sub text-[10px] tracking-widest uppercase">© 2026 ALTI Hotel. Todos los derechos reservados.</p>
      </footer>
    </div>
  );
}