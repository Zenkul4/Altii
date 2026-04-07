import { useState } from "react";
import { useNavigate } from "react-router-dom";
import BookingService from "../../services/BookingService";
import { useToast } from "../../context/ToastContext";

export default function BookingSearch() {
  const [code,    setCode]    = useState("");
  const [loading, setLoading] = useState(false);
  const navigate    = useNavigate();
  const { showToast } = useToast();

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!code.trim()) return;
    setLoading(true);
    try {
      const booking = await BookingService.getByCode(code.trim().toUpperCase());
      navigate(`/bookings/${booking.id}`);
    } catch {
      showToast("No se encontró ninguna reserva con ese código.", "error");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSearch} className="flex gap-3 items-end">
      <div className="flex-1">
        <label className="block text-[10px] tracking-[0.2em] uppercase text-text-sub mb-3">
          Buscar por código de reserva
        </label>
        <input
          type="text"
          value={code}
          onChange={e => setCode(e.target.value.toUpperCase())}
          placeholder="RES-2026-00001"
          className="input-field"
        />
      </div>
      <button
        type="submit"
        disabled={loading || !code.trim()}
        className="btn-outline py-3 px-6 text-xs mb-1"
      >
        {loading ? "..." : "Buscar"}
      </button>
    </form>
  );
}