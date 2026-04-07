import { useState } from "react";
import { useSEO } from "../../hooks/useSEO";

const info = [
  { icon:"◈", label:"Dirección",  value:"Av. Libertad 100, Santo Domingo, RD" },
  { icon:"◈", label:"Teléfono",   value:"+1 (809) 000-0000"                   },
  { icon:"◈", label:"Email",      value:"info@altihotel.com"                  },
  { icon:"◈", label:"Check-In",   value:"3:00 PM — Check-Out: 12:00 PM"       },
];

export default function ContactPage() {
  useSEO({ title:"Contacto", description:"Contáctenos para reservas, consultas o solicitudes especiales." });

  const [form, setForm] = useState({ name:"", email:"", subject:"", message:"" });
  const [sent, setSent] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement|HTMLTextAreaElement>) =>
    setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = (e: React.FormEvent) => { e.preventDefault(); setSent(true); };

  return (
    <div className="min-h-screen bg-bg-base pt-20">

      <div className="relative h-56 md:h-72 overflow-hidden">
        <img src="https://images.unsplash.com/photo-1566073771259-6a8506099945?w=1800&q=80" alt="Contact" className="w-full h-full object-cover" />
        <div className="absolute inset-0 bg-black/55" />
        <div className="absolute inset-0 flex flex-col items-center justify-center text-white text-center">
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase mb-3">Estamos para servirle</span>
          <h1 className="font-serif text-4xl md:text-6xl">Contacto</h1>
        </div>
      </div>

      <div className="max-w-6xl mx-auto px-6 py-16">
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-16">

          {/* Info */}
          <div>
            <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Información</span>
            <h2 className="font-serif text-3xl md:text-4xl text-text-main mt-3 mb-8">
              Nos encantaría<br />saber de usted
            </h2>
            <p className="text-text-sub text-sm leading-relaxed mb-12">
              Nuestro equipo de concierge está disponible las 24 horas para atender cualquier consulta,
              solicitud especial o asistencia con su reserva.
            </p>
            <div className="space-y-6">
              {info.map(item => (
                <div key={item.label} className="flex gap-4 items-start">
                  <span className="text-primary text-lg mt-0.5">{item.icon}</span>
                  <div>
                    <p className="text-[10px] tracking-[0.2em] uppercase text-text-sub mb-1">{item.label}</p>
                    <p className="text-text-main text-sm">{item.value}</p>
                  </div>
                </div>
              ))}
            </div>
            <div className="mt-12 h-48 bg-bg-card relative overflow-hidden">
              <img src="https://images.unsplash.com/photo-1524813686514-a57563d77965?w=800&q=80" alt="Location"
                className="w-full h-full object-cover opacity-30" />
              <div className="absolute inset-0 flex items-center justify-center">
                <div className="text-center">
                  <span className="text-primary text-2xl">◈</span>
                  <p className="text-text-sub text-xs tracking-widest uppercase mt-2">Santo Domingo, RD</p>
                </div>
              </div>
            </div>
          </div>

          {/* Form */}
          <div className="bg-bg-white border border-bg-card p-8 md:p-10">
            {sent ? (
              <div className="h-full flex flex-col items-center justify-center text-center py-10">
                <span className="text-primary text-5xl mb-6">✓</span>
                <h3 className="font-serif text-2xl text-text-main mb-3">Mensaje enviado</h3>
                <p className="text-text-sub text-sm leading-relaxed max-w-xs">
                  Gracias por contactarnos. Un miembro de nuestro equipo le responderá en menos de 24 horas.
                </p>
                <button onClick={()=>setSent(false)} className="btn-outline mt-8">Enviar otro mensaje</button>
              </div>
            ) : (
              <>
                <h3 className="font-serif text-2xl text-text-main mb-8">Envíenos un mensaje</h3>
                <form onSubmit={handleSubmit} className="space-y-7">
                  <div className="grid grid-cols-2 gap-6">
                    {[{label:"Nombre",name:"name",type:"text",placeholder:"Su nombre"},{label:"Email",name:"email",type:"email",placeholder:"su@email.com"}].map(f=>(
                      <div key={f.name}>
                        <label className="block text-[10px] tracking-[0.2em] uppercase text-text-sub mb-3">{f.label}</label>
                        <input type={f.type} name={f.name} value={(form as any)[f.name]} onChange={handleChange} required placeholder={f.placeholder} className="input-field" />
                      </div>
                    ))}
                  </div>
                  <div>
                    <label className="block text-[10px] tracking-[0.2em] uppercase text-text-sub mb-3">Asunto</label>
                    <input type="text" name="subject" value={form.subject} onChange={handleChange} required placeholder="¿En qué podemos ayudarle?" className="input-field" />
                  </div>
                  <div>
                    <label className="block text-[10px] tracking-[0.2em] uppercase text-text-sub mb-3">Mensaje</label>
                    <textarea name="message" value={form.message} onChange={handleChange} required rows={5}
                      placeholder="Escriba su mensaje aquí..."
                      className="w-full border-0 border-b-2 border-bg-card bg-transparent outline-none resize-none text-sm text-text-main placeholder:text-text-sub focus:border-primary transition-colors duration-200 font-sans font-light pb-2" />
                  </div>
                  <button type="submit" className="btn-primary w-full py-4">Enviar mensaje</button>
                </form>
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}