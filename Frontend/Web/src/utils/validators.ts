export const validators = {
  firstName: (v: string) =>
    !v.trim() ? "El nombre es requerido" : v.trim().length < 2 ? "Mínimo 2 caracteres" : "",

  lastName: (v: string) =>
    !v.trim() ? "El apellido es requerido" : v.trim().length < 2 ? "Mínimo 2 caracteres" : "",

  email: (v: string) => {
    if (!v.trim()) return "El email es requerido";
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v)) return "Email inválido";
    return "";
  },

  password: (v: string) => {
    if (!v) return "La contraseña es requerida";
    if (v.length < 8) return "Mínimo 8 caracteres";
    if (!/[A-Z]/.test(v)) return "Debe incluir una mayúscula";
    if (!/[0-9]/.test(v)) return "Debe incluir un número";
    return "";
  },

  phone: (v: string) =>
    v && v.length > 0 && v.length < 7 ? "Teléfono inválido" : "",
};