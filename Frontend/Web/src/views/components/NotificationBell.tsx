import { useState, useRef, useEffect } from "react";
import { useNotifications } from "../../context/NotificationContext";
import type { Notification, NotifType } from "../../context/NotificationContext";

const notifConfig: Record<NotifType, { icon: string; color: string }> = {
  booking: { icon: "◈", color: "#C9A96E" },
  payment: { icon: "✓", color: "#48bb78" },
  cancel:  { icon: "✕", color: "#fc8181" },
  system:  { icon: "◉", color: "#4a9eff" },
};

function timeAgo(date: Date): string {
  const diff = Date.now() - date.getTime();
  const m = Math.floor(diff / 60000);
  const h = Math.floor(diff / 3600000);
  if (m < 1)  return "ahora mismo";
  if (m < 60) return `hace ${m} min`;
  if (h < 24) return `hace ${h}h`;
  return date.toLocaleDateString("es-DO");
}

export default function NotificationBell() {
  const { notifications, unreadCount, markAllRead, markRead, clearAll } = useNotifications();
  const [open, setOpen] = useState(false);
  const ref  = useRef<HTMLDivElement>(null);

  // Cerrar al click fuera
  useEffect(() => {
    const handler = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", handler);
    return () => document.removeEventListener("mousedown", handler);
  }, []);

  const handleOpen = () => {
    setOpen(prev => !prev);
  };

  const handleClickNotif = (n: Notification) => {
    markRead(n.id);
  };

  return (
    <div ref={ref} style={{ position: "relative" }}>

      {/* Bell button */}
      <button
        onClick={handleOpen}
        style={{
          background:  "none",
          border:      "none",
          cursor:      "pointer",
          padding:     "6px",
          position:    "relative",
          display:     "flex",
          alignItems:  "center",
          color:       open ? "#C9A96E" : "rgba(255,255,255,0.7)",
          transition:  "color 0.2s",
          fontSize:    "18px",
        }}
        onMouseEnter={e => { if (!open) e.currentTarget.style.color = "#C9A96E"; }}
        onMouseLeave={e => { if (!open) e.currentTarget.style.color = "rgba(255,255,255,0.7)"; }}
        aria-label="Notificaciones"
      >
        {/* Bell SVG */}
        <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5">
          <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"/>
          <path d="M13.73 21a2 2 0 0 1-3.46 0"/>
        </svg>

        {/* Badge */}
        {unreadCount > 0 && (
          <span style={{
            position:   "absolute",
            top:        "2px",
            right:      "2px",
            background: "#e53e3e",
            color:      "#fff",
            fontSize:   "9px",
            fontFamily: "Montserrat, sans-serif",
            fontWeight: "600",
            minWidth:   "16px",
            height:     "16px",
            borderRadius:"8px",
            display:    "flex",
            alignItems: "center",
            justifyContent: "center",
            padding:    "0 3px",
            lineHeight: 1,
          }}>
            {unreadCount > 9 ? "9+" : unreadCount}
          </span>
        )}
      </button>

      {/* Dropdown */}
      {open && (
        <div style={{
          position:   "absolute",
          top:        "calc(100% + 12px)",
          right:      "0",
          width:      "340px",
          background: "#0C0C0C",
          border:     "1px solid rgba(255,255,255,0.08)",
          boxShadow:  "0 20px 60px rgba(0,0,0,0.6)",
          zIndex:     9998,
          animation:  "dropIn 0.2s ease both",
        }}>

          {/* Header */}
          <div style={{
            padding:        "14px 16px",
            borderBottom:   "1px solid rgba(255,255,255,0.06)",
            display:        "flex",
            justifyContent: "space-between",
            alignItems:     "center",
          }}>
            <div>
              <p style={{
                fontSize:      "10px",
                letterSpacing: "0.3em",
                textTransform: "uppercase",
                color:         "#C9A96E",
                fontFamily:    "Montserrat, sans-serif",
              }}>
                Notificaciones
              </p>
              {unreadCount > 0 && (
                <p style={{
                  fontSize:   "11px",
                  color:      "rgba(255,255,255,0.3)",
                  fontFamily: "Montserrat, sans-serif",
                  marginTop:  "2px",
                }}>
                  {unreadCount} sin leer
                </p>
              )}
            </div>
            <div style={{ display: "flex", gap: "12px" }}>
              {unreadCount > 0 && (
                <button
                  onClick={markAllRead}
                  style={{
                    fontSize:      "10px",
                    letterSpacing: "0.1em",
                    color:         "rgba(255,255,255,0.4)",
                    background:    "none",
                    border:        "none",
                    cursor:        "pointer",
                    fontFamily:    "Montserrat, sans-serif",
                    textTransform: "uppercase",
                  }}
                  onMouseEnter={e => e.currentTarget.style.color = "#C9A96E"}
                  onMouseLeave={e => e.currentTarget.style.color = "rgba(255,255,255,0.4)"}
                >
                  Marcar leídas
                </button>
              )}
              {notifications.length > 0 && (
                <button
                  onClick={clearAll}
                  style={{
                    fontSize:      "10px",
                    letterSpacing: "0.1em",
                    color:         "rgba(255,255,255,0.4)",
                    background:    "none",
                    border:        "none",
                    cursor:        "pointer",
                    fontFamily:    "Montserrat, sans-serif",
                    textTransform: "uppercase",
                  }}
                  onMouseEnter={e => e.currentTarget.style.color = "#fc8181"}
                  onMouseLeave={e => e.currentTarget.style.color = "rgba(255,255,255,0.4)"}
                >
                  Limpiar
                </button>
              )}
            </div>
          </div>

          {/* List */}
          <div style={{ maxHeight: "360px", overflowY: "auto" }}>
            {notifications.length === 0 ? (
              <div style={{
                padding:    "40px 16px",
                textAlign:  "center",
              }}>
                <p style={{
                  fontSize:      "28px",
                  marginBottom:  "10px",
                  opacity:       0.2,
                }}>◉</p>
                <p style={{
                  fontSize:      "11px",
                  letterSpacing: "0.2em",
                  textTransform: "uppercase",
                  color:         "rgba(255,255,255,0.2)",
                  fontFamily:    "Montserrat, sans-serif",
                }}>
                  Sin notificaciones
                </p>
              </div>
            ) : (
              notifications.map(n => {
                const cfg = notifConfig[n.type];
                return (
                  <div
                    key={n.id}
                    onClick={() => handleClickNotif(n)}
                    style={{
                      padding:     "14px 16px",
                      borderBottom:"1px solid rgba(255,255,255,0.04)",
                      display:     "flex",
                      gap:         "12px",
                      cursor:      "pointer",
                      background:  n.read ? "transparent" : "rgba(201,169,110,0.04)",
                      transition:  "background 0.15s",
                    }}
                    onMouseEnter={e => (e.currentTarget.style.background = "rgba(255,255,255,0.03)")}
                    onMouseLeave={e => (e.currentTarget.style.background = n.read ? "transparent" : "rgba(201,169,110,0.04)")}
                  >
                    {/* Icon */}
                    <div style={{
                      width:          "32px",
                      height:         "32px",
                      borderRadius:   "16px",
                      background:     `${cfg.color}18`,
                      border:         `1px solid ${cfg.color}30`,
                      display:        "flex",
                      alignItems:     "center",
                      justifyContent: "center",
                      flexShrink:     0,
                      color:          cfg.color,
                      fontSize:       "13px",
                    }}>
                      {cfg.icon}
                    </div>

                    {/* Content */}
                    <div style={{ flex: 1, minWidth: 0 }}>
                      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", gap: "8px" }}>
                        <p style={{
                          fontSize:      "11px",
                          fontWeight:    "500",
                          color:         n.read ? "rgba(255,255,255,0.6)" : "#fff",
                          fontFamily:    "Montserrat, sans-serif",
                          letterSpacing: "0.02em",
                        }}>
                          {n.title}
                        </p>
                        {!n.read && (
                          <div style={{
                            width:        "6px",
                            height:       "6px",
                            borderRadius: "3px",
                            background:   "#C9A96E",
                            flexShrink:   0,
                            marginTop:    "3px",
                          }} />
                        )}
                      </div>
                      <p style={{
                        fontSize:   "11px",
                        color:      "rgba(255,255,255,0.35)",
                        fontFamily: "Montserrat, sans-serif",
                        marginTop:  "3px",
                        lineHeight: "1.4",
                        whiteSpace: "nowrap",
                        overflow:   "hidden",
                        textOverflow: "ellipsis",
                      }}>
                        {n.message}
                      </p>
                      <p style={{
                        fontSize:      "10px",
                        color:         "rgba(255,255,255,0.2)",
                        fontFamily:    "Montserrat, sans-serif",
                        marginTop:     "5px",
                        letterSpacing: "0.05em",
                      }}>
                        {timeAgo(n.createdAt)}
                      </p>
                    </div>
                  </div>
                );
              })
            )}
          </div>
        </div>
      )}

      <style>{`
        @keyframes dropIn {
          from { opacity: 0; transform: translateY(-8px); }
          to   { opacity: 1; transform: translateY(0);    }
        }
      `}</style>
    </div>
  );
}