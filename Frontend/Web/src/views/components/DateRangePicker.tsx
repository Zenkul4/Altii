import { useState, useRef, useEffect } from "react";
import { createPortal } from "react-dom";

interface DateRangePickerProps {
  checkIn:     string;
  checkOut:    string;
  onChangeIn:  (date: string) => void;
  onChangeOut: (date: string) => void;
  minDate?:    string;
  theme?:      "light" | "dark";
}

const DAYS   = ["Do","Lu","Ma","Mi","Ju","Vi","Sa"];
const MONTHS = [
  "Enero","Febrero","Marzo","Abril","Mayo","Junio",
  "Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre",
];

const THEMES = {
  light: {
    triggerBg:          "#FFFFFF",
    triggerBorder:      "#E9E4ED",
    triggerBorderFocus: "#8B5FBF",
    triggerActiveBg:    "rgba(139,95,191,0.05)",
    nightsBg:           "#8B5FBF",
    nightsText:         "#FFFFFF",
    nightsLabel:        "rgba(255,255,255,0.6)",
    arrowColor:         "#D6C6E1",
    calBg:              "#FFFFFF",
    calBorder:          "rgba(139,95,191,0.3)",
    calShadow:          "0 32px 80px rgba(0,0,0,0.25), 0 0 0 1px rgba(139,95,191,0.15)",
    monthColor:         "#4A4A4A",
    yearColor:          "#8B5FBF",
    navBorder:          "#E9E4ED",
    navColor:           "#9A73B5",
    navHoverBorder:     "#8B5FBF",
    navHoverColor:      "#8B5FBF",
    dayHeaderColor:     "#9A73B5",
    accent:             "#8B5FBF",
    accentText:         "#FFFFFF",
    rangeHl:            "rgba(139,95,191,0.1)",
    rangeText:          "#61398F",
    todayDot:           "#8B5FBF",
    todayText:          "#8B5FBF",
    dayNormal:          "#4A4A4A",
    dayDisabled:        "#D6C6E1",
    dayHover:           "#61398F",
    footerBorder:       "#E9E4ED",
    clearColor:         "#878787",
    clearHover:         "#e53e3e",
    nightLabel:         "#8B5FBF",
    instructionColor:   "#8B5FBF",
    displayDay:         "#4A4A4A",
    displayAccent:      "#8B5FBF",
    displaySub:         "#878787",
    displayPlaceholder: "#D6C6E1",
    glow:               "rgba(139,95,191,0.12)",
  },
  dark: {
    triggerBg:          "#222831",
    triggerBorder:      "#393E46",
    triggerBorderFocus: "#892CDC",
    triggerActiveBg:    "rgba(137,44,220,0.08)",
    nightsBg:           "#892CDC",
    nightsText:         "#fdf6fd",
    nightsLabel:        "rgba(253,246,253,0.5)",
    arrowColor:         "#454e59",
    calBg:              "#1a2030",
    calBorder:          "rgba(137,44,220,0.35)",
    calShadow:          "0 32px 80px rgba(0,0,0,0.8), 0 0 0 1px rgba(137,44,220,0.2)",
    monthColor:         "#EEEEEE",
    yearColor:          "#BC6FF1",
    navBorder:          "#393E46",
    navColor:           "#BC6FF1",
    navHoverBorder:     "#892CDC",
    navHoverColor:      "#D9ACF5",
    dayHeaderColor:     "#BC6FF1",
    accent:             "#892CDC",
    accentText:         "#fdf6fd",
    rangeHl:            "rgba(137,44,220,0.15)",
    rangeText:          "#D9ACF5",
    todayDot:           "#BC6FF1",
    todayText:          "#BC6FF1",
    dayNormal:          "#EEEEEE",
    dayDisabled:        "#454e59",
    dayHover:           "#fff4ff",
    footerBorder:       "#2d3545",
    clearColor:         "#878787",
    clearHover:         "#fc8181",
    nightLabel:         "#BC6FF1",
    instructionColor:   "#BC6FF1",
    displayDay:         "#EEEEEE",
    displayAccent:      "#BC6FF1",
    displaySub:         "#FDEBED",
    displayPlaceholder: "#454e59",
    glow:               "rgba(137,44,220,0.2)",
  },
};

function toDate(str: string): Date | null {
  if (!str) return null;
  const [y, m, d] = str.split("-").map(Number);
  return new Date(y, m - 1, d);
}
function toStr(d: Date): string {
  return `${d.getFullYear()}-${String(d.getMonth()+1).padStart(2,"0")}-${String(d.getDate()).padStart(2,"0")}`;
}
function sameDay(a: Date, b: Date) {
  return a.getFullYear()===b.getFullYear() &&
         a.getMonth()   ===b.getMonth()    &&
         a.getDate()    ===b.getDate();
}

export default function DateRangePicker({
  checkIn, checkOut, onChangeIn, onChangeOut, minDate, theme = "light",
}: DateRangePickerProps) {
  const today       = new Date();
  const T           = THEMES[theme];
  const [open,        setOpen]        = useState(false);
  const [hovered,     setHovered]     = useState<Date | null>(null);
  const [selecting,   setSelecting]   = useState<"in"|"out">("in");
  const [viewYear,    setViewYear]    = useState(today.getFullYear());
  const [viewMonth,   setViewMonth]   = useState(today.getMonth());
  const [triggerRect, setTriggerRect] = useState<DOMRect | null>(null);
  const ref = useRef<HTMLDivElement>(null);

  const inDate  = toDate(checkIn);
  const outDate = toDate(checkOut);
  const minD    = minDate ? toDate(minDate) : today;

  // Close on outside click
  useEffect(() => {
    const h = (e: MouseEvent) => {
      const target = e.target as Node;
      if (ref.current && !ref.current.contains(target)) {
        // Also check if click is inside the portal calendar
        const portal = document.getElementById("drp-portal");
        if (portal && portal.contains(target)) return;
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", h);
    return () => document.removeEventListener("mousedown", h);
  }, []);

  // Update rect on scroll/resize
  useEffect(() => {
    const update = () => {
      if (open && ref.current) setTriggerRect(ref.current.getBoundingClientRect());
    };
    window.addEventListener("scroll", update, true);
    window.addEventListener("resize", update);
    return () => {
      window.removeEventListener("scroll", update, true);
      window.removeEventListener("resize", update);
    };
  }, [open]);

  const openCalendar = () => {
    if (ref.current) setTriggerRect(ref.current.getBoundingClientRect());
    setOpen(o => !o);
    setSelecting("in");
  };

  const prevMonth = () =>
    viewMonth === 0
      ? (setViewMonth(11), setViewYear(y => y - 1))
      : setViewMonth(m => m - 1);

  const nextMonth = () =>
    viewMonth === 11
      ? (setViewMonth(0), setViewYear(y => y + 1))
      : setViewMonth(m => m + 1);

  const handleDay = (date: Date) => {
    if (minD && date < minD && !sameDay(date, minD)) return;
    if (selecting === "in") {
      onChangeIn(toStr(date));
      if (outDate && date >= outDate) onChangeOut("");
      setSelecting("out");
    } else {
      if (date <= (inDate ?? date)) {
        onChangeIn(toStr(date));
        onChangeOut("");
        setSelecting("out");
      } else {
        onChangeOut(toStr(date));
        setSelecting("in");
        setOpen(false);
      }
    }
  };

  const isDisabled   = (d: Date) => !!(minD && d < minD && !sameDay(d, minD));
  const isStart      = (d: Date) => !!(inDate  && sameDay(d, inDate));
  const isEnd        = (d: Date) => !!(outDate && sameDay(d, outDate));
  const isInRange    = (d: Date) => {
    const end = outDate ?? (selecting === "out" ? hovered : null);
    if (!inDate || !end) return false;
    const [a, b] = inDate < end ? [inDate, end] : [end, inDate];
    return d > a && d < b;
  };
  const isHoverRange = (d: Date) => {
    if (!inDate || !hovered || selecting !== "out") return false;
    const [a, b] = inDate < hovered ? [inDate, hovered] : [hovered, inDate];
    return d > a && d < b;
  };

  const daysCount = new Date(viewYear, viewMonth + 1, 0).getDate();
  const firstDay  = new Date(viewYear, viewMonth, 1).getDay();
  const cells: (Date | null)[] = [
    ...Array(firstDay).fill(null),
    ...Array.from({ length: daysCount }, (_, i) => new Date(viewYear, viewMonth, i + 1)),
  ];
  while (cells.length % 7 !== 0) cells.push(null);

  const fmt = (str: string) => {
    const d = toDate(str);
    if (!d) return null;
    return { day: d.getDate(), mon: MONTHS[d.getMonth()].slice(0, 3).toUpperCase(), year: d.getFullYear() };
  };
  const inFmt  = fmt(checkIn);
  const outFmt = fmt(checkOut);
  const nights = inDate && outDate
    ? Math.ceil((outDate.getTime() - inDate.getTime()) / 86400000) : 0;

  // Calendar position
  const calTop  = triggerRect ? triggerRect.bottom + 10 : 0;
  const calLeft = triggerRect ? triggerRect.left + triggerRect.width / 2 : 0;

  const CalendarContent = (
    <div id="drp-portal">
      {/* Instruction */}
      <div style={{
        position:"fixed",
        top:  triggerRect ? triggerRect.bottom + 8 : 0,
        left: triggerRect ? triggerRect.left : 0,
        zIndex: 2147483647,
        pointerEvents:"none",
      }}>
        <p style={{
          fontSize:"9px", letterSpacing:"0.25em",
          textTransform:"uppercase",
          color:T.instructionColor,
          fontFamily:"Montserrat,sans-serif",
          fontWeight:600,
          background: T.calBg,
          padding:"2px 6px",
        }}>
          ◈ {selecting === "in" ? "Seleccione fecha de llegada" : "Seleccione fecha de salida"}
        </p>
      </div>

      {/* Calendar dropdown */}
      <div style={{
        position:        "fixed",
        top:             calTop + 24,
        left:            calLeft,
        transform:       "translateX(-50%)",
        background:      T.calBg,
        border:          `1px solid ${T.calBorder}`,
        boxShadow:       T.calShadow,
        zIndex:          2147483647,
        minWidth:        "340px",
        animation:       "calDrop 0.28s cubic-bezier(0.34,1.2,0.64,1) both",
        backdropFilter:  "none",
        WebkitBackdropFilter: "none",
      }}>

        {/* Month nav */}
        <div style={{
          display:"flex", alignItems:"center", justifyContent:"space-between",
          padding:"20px 24px 16px",
          borderBottom:`1px solid ${T.footerBorder}`,
        }}>
          <NavBtn onClick={prevMonth} T={T}>‹</NavBtn>
          <div style={{ textAlign:"center" }}>
            <p style={{
              fontFamily:"'Cormorant Garamond',serif",
              fontSize:"22px", color:T.monthColor,
              lineHeight:1, letterSpacing:"0.04em", fontWeight:500,
            }}>
              {MONTHS[viewMonth]}
            </p>
            <p style={{
              fontFamily:"Montserrat,sans-serif",
              fontSize:"10px", color:T.yearColor,
              letterSpacing:"0.3em", marginTop:"3px", fontWeight:600,
            }}>
              {viewYear}
            </p>
          </div>
          <NavBtn onClick={nextMonth} T={T}>›</NavBtn>
        </div>

        {/* Day headers */}
        <div style={{ display:"grid", gridTemplateColumns:"repeat(7,1fr)", padding:"14px 16px 4px" }}>
          {DAYS.map(d => (
            <div key={d} style={{
              textAlign:"center", fontFamily:"Montserrat,sans-serif",
              fontSize:"9px", letterSpacing:"0.2em",
              textTransform:"uppercase", color:T.dayHeaderColor,
              paddingBottom:"8px", fontWeight:700,
            }}>
              {d}
            </div>
          ))}
        </div>

        {/* Days grid */}
        <div style={{ display:"grid", gridTemplateColumns:"repeat(7,1fr)", padding:"0 16px 20px", gap:"1px" }}>
          {cells.map((date, i) => {
            if (!date) return <div key={i} />;
            const dis   = isDisabled(date);
            const start = isStart(date);
            const end   = isEnd(date);
            const inR   = isInRange(date);
            const hovR  = isHoverRange(date);
            const act   = start || end;
            const isTod = sameDay(date, today);

            return (
              <div
                key={i}
                onClick={() => !dis && handleDay(date)}
                onMouseEnter={() => !dis && setHovered(date)}
                onMouseLeave={() => setHovered(null)}
                style={{
                  position:"relative", height:"42px",
                  display:"flex", alignItems:"center", justifyContent:"center",
                  cursor: dis ? "not-allowed" : "pointer",
                  background: act ? T.accent : (inR || hovR) ? T.rangeHl : "transparent",
                  borderRadius: start ? "2px 0 0 2px" : end ? "0 2px 2px 0" : "0",
                  transition:"background 0.12s",
                }}
              >
                <DayLabel n={date.getDate()} act={act} dis={dis} isTod={isTod} inR={inR || hovR} T={T} />
                {isTod && !act && (
                  <div style={{
                    position:"absolute", bottom:"5px", left:"50%",
                    transform:"translateX(-50%)",
                    width:"3px", height:"3px",
                    borderRadius:"50%", background:T.todayDot,
                  }} />
                )}
              </div>
            );
          })}
        </div>

        {/* Footer */}
        {(checkIn || checkOut) && (
          <div style={{
            borderTop:`1px solid ${T.footerBorder}`,
            padding:"12px 20px",
            display:"flex", justifyContent:"space-between", alignItems:"center",
          }}>
            <button
              type="button"
              onClick={() => { onChangeIn(""); onChangeOut(""); setSelecting("in"); }}
              style={{
                fontFamily:"Montserrat,sans-serif", fontSize:"9px",
                letterSpacing:"0.2em", textTransform:"uppercase",
                fontWeight:600, color:T.clearColor,
                background:"none", border:"none", cursor:"pointer",
                transition:"color 0.2s",
              }}
              onMouseEnter={e => e.currentTarget.style.color = T.clearHover}
              onMouseLeave={e => e.currentTarget.style.color = T.clearColor}
            >
              Limpiar fechas
            </button>
            {nights > 0 && (
              <p style={{
                fontFamily:"Montserrat,sans-serif", fontSize:"10px",
                letterSpacing:"0.2em", textTransform:"uppercase",
                color:T.nightLabel, fontWeight:600,
              }}>
                {nights} {nights === 1 ? "noche" : "noches"}
              </p>
            )}
          </div>
        )}
      </div>

      <style>{`
        @keyframes calDrop {
          from { opacity:0; transform:translateX(-50%) translateY(-10px) scale(0.97); }
          to   { opacity:1; transform:translateX(-50%) translateY(0) scale(1); }
        }
      `}</style>
    </div>
  );

  return (
    <div ref={ref} style={{ position:"relative", userSelect:"none" }}>

      {/* ── Trigger ── */}
      <button
        type="button"
        onClick={openCalendar}
        style={{
          display:    "flex",
          alignItems: "stretch",
          background: T.triggerBg,
          border:     `1px solid ${open ? T.triggerBorderFocus : T.triggerBorder}`,
          cursor:     "pointer",
          padding:    0,
          width:      "100%",
          overflow:   "hidden",
          transition: "border-color 0.25s, box-shadow 0.25s",
          boxShadow:  open ? `0 0 0 3px ${T.glow}` : "none",
        }}
      >
        <TriggerPanel label="Check-In"  fmt={inFmt}  active={selecting === "in"  && open} T={T} side="right" />

        {/* Nights badge */}
        <div style={{
          display:"flex", flexDirection:"column", alignItems:"center",
          justifyContent:"center", padding:"0 16px",
          background: nights > 0 ? T.nightsBg : "transparent",
          minWidth:"58px", transition:"background 0.3s",
        }}>
          {nights > 0 ? (
            <>
              <span style={{ fontFamily:"'Cormorant Garamond',serif", fontSize:"24px", color:T.nightsText, lineHeight:1 }}>
                {nights}
              </span>
              <span style={{ fontFamily:"Montserrat,sans-serif", fontSize:"8px", letterSpacing:"0.2em", textTransform:"uppercase", color:T.nightsLabel, marginTop:"2px" }}>
                {nights === 1 ? "noche" : "noches"}
              </span>
            </>
          ) : (
            <span style={{ color:T.arrowColor, fontSize:"14px" }}>→</span>
          )}
        </div>

        <TriggerPanel label="Check-Out" fmt={outFmt} active={selecting === "out" && open} T={T} side="left" />
      </button>

      {/* ── Portal ── */}
      {open && triggerRect && createPortal(CalendarContent, document.body)}
    </div>
  );
}

/* ── TriggerPanel ─────────────────────────────────────── */
function TriggerPanel({ label, fmt, active, T, side }: {
  label:  string;
  fmt:    { day: number; mon: string; year: number } | null;
  active: boolean;
  T:      typeof THEMES["light"];
  side:   "left" | "right";
}) {
  return (
    <div style={{
      flex:1, padding:"14px 20px", textAlign:"left",
      ...(side === "right"
        ? { borderRight:`1px solid ${T.triggerBorder}` }
        : { borderLeft: `1px solid ${T.triggerBorder}` }),
      background: active ? T.triggerActiveBg : "transparent",
      transition:"background 0.2s",
    }}>
      <p style={{
        fontSize:"9px", letterSpacing:"0.3em", textTransform:"uppercase",
        color:T.displaySub, fontFamily:"Montserrat,sans-serif",
        marginBottom:"6px", fontWeight:600,
      }}>
        {label}
      </p>
      {fmt ? (
        <div style={{ display:"flex", alignItems:"baseline", gap:"6px" }}>
          <span style={{
            fontFamily:"'Cormorant Garamond',serif",
            fontSize:"30px", color:T.displayDay, lineHeight:1, fontWeight:500,
          }}>
            {fmt.day}
          </span>
          <div>
            <p style={{
              fontFamily:"Montserrat,sans-serif", fontSize:"9px",
              color:T.displayAccent, letterSpacing:"0.15em",
              textTransform:"uppercase", fontWeight:700,
            }}>
              {fmt.mon}
            </p>
            <p style={{
              fontFamily:"Montserrat,sans-serif", fontSize:"9px",
              color:T.displaySub, marginTop:"1px", fontWeight:500,
            }}>
              {fmt.year}
            </p>
          </div>
        </div>
      ) : (
        <p style={{
          fontFamily:"Montserrat,sans-serif", fontSize:"13px",
          color:T.displayPlaceholder, fontWeight:400,
        }}>
          Seleccionar
        </p>
      )}
    </div>
  );
}

/* ── NavBtn ───────────────────────────────────────────── */
function NavBtn({ onClick, T, children }: {
  onClick:  () => void;
  T:        typeof THEMES["light"];
  children: React.ReactNode;
}) {
  const [hov, setHov] = useState(false);
  return (
    <button
      type="button"
      onClick={onClick}
      onMouseEnter={() => setHov(true)}
      onMouseLeave={() => setHov(false)}
      style={{
        background:"none",
        border:`1px solid ${hov ? T.navHoverBorder : T.navBorder}`,
        color: hov ? T.navHoverColor : T.navColor,
        cursor:"pointer", width:"32px", height:"32px",
        display:"flex", alignItems:"center", justifyContent:"center",
        fontSize:"16px", transition:"all 0.2s", fontWeight:700,
      }}
    >
      {children}
    </button>
  );
}

/* ── DayLabel ─────────────────────────────────────────── */
function DayLabel({ n, act, dis, isTod, inR, T }: {
  n:     number;
  act:   boolean;
  dis:   boolean;
  isTod: boolean;
  inR:   boolean;
  T:     typeof THEMES["light"];
}) {
  const [hov, setHov] = useState(false);
  const color = dis ? T.dayDisabled
    : act            ? T.accentText
    : isTod          ? T.todayText
    : inR            ? T.rangeText
    : hov            ? T.dayHover
    : T.dayNormal;

  return (
    <span
      onMouseEnter={() => setHov(true)}
      onMouseLeave={() => setHov(false)}
      style={{
        position:"relative", zIndex:1,
        fontFamily: act ? "'Cormorant Garamond',serif" : "Montserrat,sans-serif",
        fontSize:   act ? "17px" : "12px",
        fontWeight: 500,
        color,
        transition:"color 0.12s",
        lineHeight:1,
      }}
    >
      {n}
    </span>
  );
}