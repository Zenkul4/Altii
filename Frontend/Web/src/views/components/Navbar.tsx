import { useState } from "react";
import { Link, useLocation } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { useTheme } from "../../context/ThemeContext";
import useAuthController from "../../controllers/useAuthController";
import NotificationBell from "./NotificationBell";
import ThemeToggle from "./ThemeToggle";

export default function Navbar() {
  const { user }         = useAuth();
  const { theme }        = useTheme();
  const { handleLogout } = useAuthController();
  const location         = useLocation();
  const [menuOpen, setMenuOpen] = useState(false);

  const isDark = theme === "dark";

  /*
   * Glass effect:
   *   dark  → fondo oscuro semitransparente + textos claros
   *   light → fondo blanco semitransparente + textos oscuros
   * backdrop-blur crea contraste con cualquier contenido detrás.
   */
  const glassBase   = isDark
    ? "bg-[#222831]/75 border-white/5"
    : "bg-white/75 border-black/5";

  const textNormal  = isDark ? "text-white/75"  : "text-[#4A4A4A]/80";
  const textHover   = isDark ? "hover:text-white": "hover:text-[#4A4A4A]";
  const textActive  = "text-primary";

  const logoName    = isDark ? "text-white"      : "text-[#4A4A4A]";
  const logoSub     = "text-primary";

  const burgerColor = isDark ? "bg-white"        : "bg-[#4A4A4A]";

  const btnSalir    = isDark
    ? "border-white/25 text-white/75 hover:border-white hover:text-white"
    : "border-[#4A4A4A]/30 text-[#4A4A4A]/70 hover:border-[#4A4A4A] hover:text-[#4A4A4A]";

  const btnAcceder  = isDark
    ? "text-white/75 hover:text-white"
    : "text-[#4A4A4A]/70 hover:text-[#4A4A4A]";

  const navLinks = [
    { to: "/",           label: "Inicio"       },
    { to: "/rooms",      label: "Habitaciones" },
    { to: "/services",   label: "Servicios"    },
    { to: "/contact",    label: "Contacto"     },
    { to: "/about",      label: "Nosotros"     },
    { to: "/gallery", label: "Galería" },
    ...(user ? [{ to: "/my-bookings", label: "Mis Reservas" }] : []),
  ];

  return (
    <>
      <nav className={`
        sticky top-0 z-50
        backdrop-blur-md
        border-b
        transition-colors duration-300
        ${glassBase}
      `}>
        <div className="max-w-7xl mx-auto px-6 lg:px-12">
          <div className="flex items-center justify-between h-20">

            {/* ── Logo ── */}
            <Link to="/" className="flex flex-col items-start group">
              <span className={`
                font-serif text-2xl tracking-[0.08em] leading-none
                transition-colors duration-300
                group-hover:text-primary
                ${logoName}
              `}>
                ALTI
              </span>
              <span className={`
                text-[9px] tracking-[0.45em] uppercase mt-0.5
                ${logoSub}
              `}>
                Hotel & Resort
              </span>
            </Link>

            {/* ── Desktop links ── */}
            <div className="hidden lg:flex items-center gap-10">
              {navLinks.map(link => {
                const isActive = location.pathname === link.to;
                return (
                  <Link
                    key={link.to}
                    to={link.to}
                    className={`
                      relative text-xs tracking-[0.15em] uppercase
                      transition-colors duration-300
                      ${isActive ? textActive : `${textNormal} ${textHover}`}
                    `}
                  >
                    {link.label}
                    {/* Underline indicator */}
                    <span className={`
                      absolute -bottom-1 left-0 h-px bg-primary
                      transition-all duration-300
                      ${isActive ? "w-full" : "w-0 group-hover:w-full"}
                    `} />
                  </Link>
                );
              })}
            </div>

            {/* ── Auth + tools ── */}
            <div className="hidden lg:flex items-center gap-4">
              <ThemeToggle />

              {user ? (
                <div className="flex items-center gap-4">
                  <NotificationBell />

                  {/* Avatar + nombre */}
                  <Link
                    to="/profile"
                    className={`
                      flex items-center gap-2
                      transition-colors duration-300
                      ${textNormal} ${textHover}
                    `}
                  >
                    <div className="w-8 h-8 rounded-full bg-primary/20 border border-primary/30 flex items-center justify-center">
                      <span className="text-primary text-xs font-medium font-serif">
                        {user.fullName.charAt(0)}
                      </span>
                    </div>
                    <span className="text-xs tracking-widest">
                      {user.fullName.split(" ")[0]}
                    </span>
                  </Link>

                  <button
                    onClick={handleLogout}
                    className={`
                      text-[11px] py-2 px-5 tracking-[0.2em] uppercase
                      border transition-all duration-300
                      ${btnSalir}
                    `}
                  >
                    Salir
                  </button>
                </div>
              ) : (
                <div className="flex items-center gap-4">
                  <Link
                    to="/login"
                    className={`
                      text-xs tracking-[0.15em] uppercase
                      transition-colors duration-300
                      ${btnAcceder}
                    `}
                  >
                    Acceder
                  </Link>
                  <Link to="/register" className="btn-primary text-[11px] py-2.5 px-6">
                    Registrar
                  </Link>
                </div>
              )}
            </div>

            {/* ── Mobile hamburger ── */}
            <button
              className="lg:hidden p-2 flex flex-col justify-center gap-1.5"
              onClick={() => setMenuOpen(!menuOpen)}
              aria-label={menuOpen ? "Cerrar menú" : "Abrir menú"}
              aria-expanded={menuOpen}
            >
              <span className={`block w-6 h-px transition-all duration-300 ${burgerColor} ${menuOpen ? "rotate-45 translate-y-2" : ""}`} />
              <span className={`block w-6 h-px transition-all duration-300 ${burgerColor} ${menuOpen ? "opacity-0 scale-x-0" : ""}`} />
              <span className={`block w-6 h-px transition-all duration-300 ${burgerColor} ${menuOpen ? "-rotate-45 -translate-y-2" : ""}`} />
            </button>

          </div>
        </div>
      </nav>

      {/* ── Mobile menu fullscreen ── */}
      <div className={`
        fixed inset-0 z-40
        flex flex-col items-center justify-center gap-8
        transition-all duration-500
        backdrop-blur-md
        ${isDark ? "bg-[#222831]/95" : "bg-white/95"}
        ${menuOpen ? "opacity-100 pointer-events-auto" : "opacity-0 pointer-events-none"}
      `}>
        {/* Close button */}
        <button
          onClick={() => setMenuOpen(false)}
          className={`absolute top-6 right-6 text-2xl transition-colors duration-200 ${
            isDark ? "text-white/50 hover:text-white" : "text-[#4A4A4A]/50 hover:text-[#4A4A4A]"
          }`}
        >
          ✕
        </button>

        {/* Links */}
        {[
          { to: "/",            label: "Inicio"        },
          { to: "/rooms",       label: "Habitaciones"  },
          { to: "/services",    label: "Servicios"     },
          { to: "/contact",     label: "Contacto"      },
          { to: "/about",       label: "Nosotros"      },
          { to: "/gallery", label: "Galería" },
          ...(user ? [
            { to: "/my-bookings", label: "Mis Reservas" },
            { to: "/profile",     label: "Mi Perfil"    },
          ] : []),
        ].map(link => (
          <Link
            key={link.to}
            to={link.to}
            onClick={() => setMenuOpen(false)}
            className={`
              font-serif text-4xl transition-colors duration-300
              ${location.pathname === link.to
                ? "text-primary"
                : isDark
                ? "text-white hover:text-primary"
                : "text-[#4A4A4A] hover:text-primary"
              }
            `}
          >
            {link.label}
          </Link>
        ))}

        {/* Divider */}
        <div className="w-12 h-px bg-primary/40" />

        <ThemeToggle />

        <div className="flex items-center gap-6">
          {user ? (
            <button
              onClick={() => { handleLogout(); setMenuOpen(false); }}
              className="btn-outline"
            >
              Salir
            </button>
          ) : (
            <>
              <Link to="/login"    onClick={() => setMenuOpen(false)} className="btn-outline">Acceder</Link>
              <Link to="/register" onClick={() => setMenuOpen(false)} className="btn-primary">Registrar</Link>
            </>
          )}
        </div>
      </div>
    </>
  );
}