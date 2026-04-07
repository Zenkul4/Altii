interface DividerProps {
  from: string;
  to:   string;
  flip?: boolean;
}

export default function SectionDivider({ from, to, flip = false }: DividerProps) {
  return (
    <div style={{ position: "relative", height: "70px", background: from, overflow: "hidden" }}>
      <svg
        viewBox="0 0 1440 70"
        preserveAspectRatio="none"
        style={{
          position: "absolute",
          bottom: 0,
          width: "100%",
          height: "100%",
          transform: flip ? "scaleX(-1)" : "none",
        }}
        fill={to}
        xmlns="http://www.w3.org/2000/svg"
      >
        <path d="M0,40 C360,80 1080,0 1440,40 L1440,70 L0,70 Z" />
      </svg>
    </div>
  );
}