import { createContext, useContext, useState, useCallback } from "react";
import type { ReactNode } from "react";

export type ToastType = "success" | "error" | "info" | "warning";

interface Toast {
  id:      number;
  message: string;
  type:    ToastType;
}

interface ToastContextType {
  showToast: (message: string, type?: ToastType) => void;
}

const ToastContext = createContext<ToastContextType | null>(null);

const config: Record<ToastType, { bg: string; border: string; icon: string; label: string }> = {
  success: { bg: "#0C0C0C", border: "#C9A96E", icon: "✓", label: "Éxito"         },
  error:   { bg: "#1a0a0a", border: "#e53e3e", icon: "✕", label: "Error"         },
  info:    { bg: "#0a0f1a", border: "#4a9eff", icon: "◈", label: "Información"   },
  warning: { bg: "#1a1200", border: "#f6ad55", icon: "⚠", label: "Advertencia"  },
};

const DURATION = 4000;

export const ToastProvider = ({ children }: { children: ReactNode }) => {
  const [toasts, setToasts] = useState<Toast[]>([]);

  const showToast = useCallback((message: string, type: ToastType = "success") => {
    const id = Date.now();
    setToasts(prev => [...prev, { id, message, type }]);
    setTimeout(() => {
      setToasts(prev => prev.filter(t => t.id !== id));
    }, DURATION);
  }, []);

  const dismiss = (id: number) =>
    setToasts(prev => prev.filter(t => t.id !== id));

  return (
    <ToastContext.Provider value={{ showToast }}>
      {children}

      <div style={{
        position:       "fixed",
        bottom:         "1.5rem",
        right:          "1.5rem",
        zIndex:         9999,
        display:        "flex",
        flexDirection:  "column",
        gap:            "10px",
        pointerEvents:  "none",
      }}>
        {toasts.map(t => {
          const c = config[t.type];
          return (
            <div
              key={t.id}
              style={{
                background:   c.bg,
                borderLeft:   `3px solid ${c.border}`,
                color:        "#fff",
                padding:      "0",
                minWidth:     "300px",
                maxWidth:     "400px",
                animation:    "toastIn 0.35s cubic-bezier(0.34,1.56,0.64,1) both",
                pointerEvents:"all",
                overflow:     "hidden",
                boxShadow:    "0 8px 32px rgba(0,0,0,0.4)",
              }}
            >
              {/* Content */}
              <div style={{ display: "flex", alignItems: "flex-start", gap: "12px", padding: "14px 16px" }}>
                <span style={{
                  color:      c.border,
                  fontSize:   "15px",
                  flexShrink: 0,
                  marginTop:  "1px",
                  fontWeight: "bold",
                }}>
                  {c.icon}
                </span>
                <div style={{ flex: 1 }}>
                  <p style={{
                    fontSize:      "10px",
                    letterSpacing: "0.2em",
                    textTransform: "uppercase",
                    color:         c.border,
                    marginBottom:  "3px",
                    fontFamily:    "Montserrat, sans-serif",
                  }}>
                    {c.label}
                  </p>
                  <p style={{
                    fontSize:   "13px",
                    lineHeight: "1.45",
                    color:      "rgba(255,255,255,0.85)",
                    fontFamily: "Montserrat, sans-serif",
                    fontWeight: "300",
                  }}>
                    {t.message}
                  </p>
                </div>
                <button
                  onClick={() => dismiss(t.id)}
                  style={{
                    color:      "rgba(255,255,255,0.3)",
                    fontSize:   "14px",
                    background: "none",
                    border:     "none",
                    cursor:     "pointer",
                    padding:    "0",
                    flexShrink: 0,
                  }}
                  onMouseEnter={e => (e.currentTarget.style.color = "rgba(255,255,255,0.8)")}
                  onMouseLeave={e => (e.currentTarget.style.color = "rgba(255,255,255,0.3)")}
                >
                  ✕
                </button>
              </div>

              {/* Progress bar */}
              <div style={{
                height:     "2px",
                background: "rgba(255,255,255,0.08)",
                position:   "relative",
                overflow:   "hidden",
              }}>
                <div style={{
                  position:        "absolute",
                  top:             0,
                  left:            0,
                  height:          "100%",
                  background:      c.border,
                  width:           "100%",
                  animation:       `toastProgress ${DURATION}ms linear forwards`,
                  transformOrigin: "left",
                }} />
              </div>
            </div>
          );
        })}
      </div>

      <style>{`
        @keyframes toastIn {
          from { opacity: 0; transform: translateX(24px) scale(0.95); }
          to   { opacity: 1; transform: translateX(0)    scale(1);    }
        }
        @keyframes toastProgress {
          from { transform: scaleX(1); }
          to   { transform: scaleX(0); }
        }
      `}</style>
    </ToastContext.Provider>
  );
};

export const useToast = () => {
  const ctx = useContext(ToastContext);
  if (!ctx) throw new Error("useToast must be used within ToastProvider");
  return ctx;
};