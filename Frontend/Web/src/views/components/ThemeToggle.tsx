import { useTheme } from "../../context/ThemeContext";

export default function ThemeToggle() {
  const { theme, toggleTheme } = useTheme();
  const isDark = theme === "dark";

  return (
    <button
      onClick={toggleTheme}
      aria-label="Cambiar tema"
      style={{
        background:   "none",
        border:       `1px solid ${isDark ? "rgba(188,111,241,0.3)" : "rgba(139,95,191,0.25)"}`,
        cursor:       "pointer",
        width:        "40px",
        height:       "22px",
        borderRadius: "11px",
        padding:      "2px",
        position:     "relative",
        transition:   "all 0.3s",
        flexShrink:   0,
        backgroundColor: isDark ? "rgba(137,44,220,0.2)" : "rgba(139,95,191,0.08)",
      }}
    >
      {/* Track */}
      <div style={{
        position:     "absolute",
        inset:        "2px",
        borderRadius: "10px",
        background:   isDark
          ? "linear-gradient(90deg, #892CDC22, #BC6FF133)"
          : "linear-gradient(90deg, #8B5FBF22, #D6C6E133)",
      }} />

      {/* Thumb */}
      <div style={{
        position:     "absolute",
        top:          "3px",
        left:         isDark ? "calc(100% - 19px)" : "3px",
        width:        "14px",
        height:       "14px",
        borderRadius: "50%",
        background:   isDark ? "#BC6FF1" : "#8B5FBF",
        transition:   "left 0.3s cubic-bezier(0.34,1.56,0.64,1)",
        display:      "flex",
        alignItems:   "center",
        justifyContent:"center",
        fontSize:     "8px",
        boxShadow:    isDark
          ? "0 0 6px rgba(188,111,241,0.6)"
          : "0 0 6px rgba(139,95,191,0.4)",
      }}>
        {isDark ? "☽" : "☀"}
      </div>
    </button>
  );
}