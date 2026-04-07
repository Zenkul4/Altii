import { Link } from "react-router-dom";

export default function NotFoundPage() {
  return (
    <div className="min-h-screen bg-bg-base flex items-center justify-center px-6">
      <div className="text-center">
        <p className="font-serif text-[180px] md:text-[220px] text-text-sub/10 leading-none select-none">404</p>
        <div className="-mt-16 relative z-10">
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Página no encontrada</span>
          <h1 className="font-serif text-4xl md:text-5xl text-text-main mt-3 mb-4">Esta habitación no existe</h1>
          <p className="text-text-sub text-sm tracking-wide mb-10 max-w-sm mx-auto">
            La página que busca no está disponible o ha sido movida.
          </p>
          <div className="flex gap-4 justify-center flex-wrap">
            <Link to="/"      className="btn-primary px-10 py-3.5">Volver al inicio</Link>
            <Link to="/rooms" className="btn-outline px-10 py-3.5">Ver habitaciones</Link>
          </div>
        </div>
      </div>
    </div>
  );
}