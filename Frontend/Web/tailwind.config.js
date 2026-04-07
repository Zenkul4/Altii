/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  darkMode: ["class", '[data-theme="dark"]'],
  theme: {
    extend: {
      fontWeight: {
      thin:       "100",
      extralight: "200",
      light:      "300",
      normal:     "400",   // ← base
      medium:     "500",
      semibold:   "600",
      bold:       "700",
    },
      colors: {
        primary:  "var(--primary-100)",
        "primary-dark": "var(--primary-200)",
        accent:   "var(--accent-100)",
        "accent-2": "var(--accent-200)",
        "text-main": "var(--text-100)",
        "text-sub":  "var(--text-200)",
        "bg-base":   "var(--bg-100)",
        "bg-card":   "var(--bg-200)",
        "bg-white":  "var(--bg-300)",
        // legacy aliases para no romper páginas existentes
        gold:     "var(--primary-100)",
        dark:     "var(--bg-100)",
        cream:    "var(--bg-100)",
        warm:     "var(--bg-200)",
        muted:    "var(--text-200)",
      },
      fontFamily: {
        serif: ["'Cormorant Garamond'", "serif"],
        sans:  ["'Montserrat'",         "sans-serif"],
      },
      letterSpacing: {
        widest: "0.2em",
      },
    },
  },
  plugins: [],
};