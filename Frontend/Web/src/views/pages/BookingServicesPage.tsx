import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import AdditionalServiceService from "../../services/AdditionalServiceService";
import type { AdditionalServiceDto, CreateBookingServiceDto } from "../../models/AdditionalService";
import type { BookingResponseDto } from "../../models/Booking";
import { useSEO } from "../../hooks/useSEO";

interface SelectedService {
  service: AdditionalServiceDto;
  quantity: number;
}

export default function BookingServicesPage() {
  useSEO({ title: "Servicios adicionales", description: "Agregue servicios a su reserva." });

  const navigate = useNavigate();
  const { user } = useAuth();
  const { state } = useLocation() as {
    state: { booking: BookingResponseDto; total: number } | null;
  };

  const [services, setServices] = useState<AdditionalServiceDto[]>([]);
  const [selected, setSelected] = useState<SelectedService[]>([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (!state?.booking) {
      navigate("/my-bookings");
      return;
    }
    const load = async () => {
      try {
        const result = await AdditionalServiceService.getAllActive();
        setServices(result);
      } catch (err: unknown) {
        const message = err instanceof Error ? err.message : "Error al cargar servicios.";
        setError(message);
      } finally {
        setLoading(false);
      }
    };
    load();
  }, []);

  if (!state?.booking) return null;

  const { booking, total: roomTotal } = state;
  const servicesTotal = selected.reduce((sum, s) => sum + s.service.price * s.quantity, 0);
  const grandTotal = roomTotal + servicesTotal;

  const toggleService = (service: AdditionalServiceDto) => {
    setSelected((prev) => {
      const exists = prev.find((s) => s.service.id === service.id);
      if (exists) {
        return prev.filter((s) => s.service.id !== service.id);
      }
      return [...prev, { service, quantity: 1 }];
    });
  };

  const updateQuantity = (serviceId: number, quantity: number) => {
    if (quantity < 1) return;
    setSelected((prev) =>
      prev.map((s) => (s.service.id === serviceId ? { ...s, quantity } : s))
    );
  };

  const isSelected = (serviceId: number) =>
    selected.some((s) => s.service.id === serviceId);

  const handleContinue = async () => {
    if (!user) return;
    setSaving(true);
    setError("");

    try {
      for (const item of selected) {
        const dto: CreateBookingServiceDto = {
          bookingId: booking.id,
          serviceId: item.service.id,
          registeredById: user.id,
          quantity: item.quantity,
        };
        await AdditionalServiceService.addToBooking(dto);
      }

      navigate(`/bookings/${booking.id}/pay`, {
        state: { booking, total: grandTotal },
      });
    } catch (err: unknown) {
      const message = err instanceof Error ? err.message : "Error al agregar servicios.";
      setError(message);
    } finally {
      setSaving(false);
    }
  };

  const handleSkip = () => {
    navigate(`/bookings/${booking.id}/pay`, {
      state: { booking, total: roomTotal },
    });
  };

  return (
    <div className="min-h-screen bg-bg-base pt-20">
      <div className="max-w-5xl mx-auto px-6 py-16">
        <button
          onClick={() => navigate(-1)}
          className="text-[10px] tracking-[0.2em] uppercase text-text-sub hover:text-primary transition-colors flex items-center gap-2 mb-10"
        >
          ← Volver
        </button>

        <div className="flex items-center gap-6 mb-12">
          <div className="h-px flex-1 bg-bg-card" />
          <span className="text-primary text-[10px] tracking-[0.4em] uppercase">
            Servicios Adicionales
          </span>
          <div className="h-px flex-1 bg-bg-card" />
        </div>

        {/* Progress indicator */}
        <div className="flex items-center justify-center gap-3 mb-12">
          {["Habitación", "Servicios", "Pago", "Confirmación"].map((step, i) => (
            <div key={step} className="flex items-center gap-3">
              <div className="flex items-center gap-2">
                <div
                  className={`w-8 h-8 rounded-full flex items-center justify-center text-xs font-medium ${
                    i <= 1
                      ? "bg-primary text-white"
                      : "bg-bg-card text-text-sub"
                  }`}
                >
                  {i < 1 ? "✓" : i + 1}
                </div>
                <span
                  className={`text-[10px] tracking-[0.15em] uppercase font-semibold ${
                    i <= 1 ? "text-primary" : "text-text-sub"
                  }`}
                >
                  {step}
                </span>
              </div>
              {i < 3 && <div className="w-8 h-px bg-bg-card" />}
            </div>
          ))}
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-10">
          {/* Services list */}
          <div className="lg:col-span-2">
            <div className="bg-bg-white border border-bg-card p-8">
              <h2 className="font-serif text-2xl text-text-main mb-2">
                Mejore su estadía
              </h2>
              <p className="text-text-sub text-xs tracking-wide mb-8">
                Seleccione los servicios que desea agregar a su reserva
              </p>

              {loading ? (
                <div className="flex justify-center py-12">
                  <div className="w-8 h-8 border-2 border-primary border-t-transparent rounded-full animate-spin" />
                </div>
              ) : services.length === 0 ? (
                <div className="text-center py-12">
                  <p className="text-text-sub text-sm">
                    No hay servicios disponibles en este momento.
                  </p>
                </div>
              ) : (
                <div className="space-y-4">
                  {services.map((service) => {
                    const active = isSelected(service.id);
                    const item = selected.find(
                      (s) => s.service.id === service.id
                    );

                    return (
                      <div
                        key={service.id}
                        className={`border p-5 cursor-pointer transition-all duration-200 ${
                          active
                            ? "border-primary bg-primary/5"
                            : "border-bg-card hover:border-primary/30"
                        }`}
                        onClick={() => toggleService(service)}
                      >
                        <div className="flex items-start justify-between">
                          <div className="flex items-start gap-4">
                            <div
                              className={`w-6 h-6 rounded-full border-2 flex items-center justify-center flex-shrink-0 mt-0.5 transition-all ${
                                active
                                  ? "border-primary bg-primary"
                                  : "border-bg-card"
                              }`}
                            >
                              {active && (
                                <span className="text-white text-xs">✓</span>
                              )}
                            </div>
                            <div>
                              <h3 className="text-sm text-text-main font-medium">
                                {service.name}
                              </h3>
                              {service.description && (
                                <p className="text-text-sub text-xs mt-1 leading-relaxed">
                                  {service.description}
                                </p>
                              )}
                            </div>
                          </div>
                          <div className="text-right flex-shrink-0 ml-4">
                            <p className="font-serif text-lg text-primary font-medium">
                              ${service.price}
                            </p>
                            <p className="text-[9px] text-text-sub tracking-widest uppercase">
                              por unidad
                            </p>
                          </div>
                        </div>

                        {active && item && (
                          <div
                            className="flex items-center gap-3 mt-4 pt-4 border-t border-bg-card"
                            onClick={(e) => e.stopPropagation()}
                          >
                            <span className="text-[10px] tracking-[0.2em] uppercase text-text-sub">
                              Cantidad:
                            </span>
                            <button
                              onClick={() =>
                                updateQuantity(service.id, item.quantity - 1)
                              }
                              className="w-8 h-8 border border-bg-card flex items-center justify-center text-text-sub hover:border-primary hover:text-primary transition-colors"
                            >
                              −
                            </button>
                            <span className="text-sm text-text-main font-medium w-8 text-center">
                              {item.quantity}
                            </span>
                            <button
                              onClick={() =>
                                updateQuantity(service.id, item.quantity + 1)
                              }
                              className="w-8 h-8 border border-bg-card flex items-center justify-center text-text-sub hover:border-primary hover:text-primary transition-colors"
                            >
                              +
                            </button>
                            <span className="ml-auto font-serif text-primary font-medium">
                              ${service.price * item.quantity}
                            </span>
                          </div>
                        )}
                      </div>
                    );
                  })}
                </div>
              )}
            </div>
          </div>

          {/* Summary sidebar */}
          <div className="lg:col-span-1">
            <div className="bg-bg-white border border-bg-card p-6 sticky top-24">
              <h3 className="font-serif text-xl text-text-main mb-6">
                Resumen
              </h3>

              <div className="space-y-3 mb-6">
                <div className="flex justify-between pb-3 border-b border-bg-card">
                  <span className="text-[10px] tracking-[0.2em] uppercase text-text-sub">
                    Habitación
                  </span>
                  <span className="text-sm text-text-main font-medium">
                    ${roomTotal}
                  </span>
                </div>

                {selected.map((item) => (
                  <div
                    key={item.service.id}
                    className="flex justify-between pb-3 border-b border-bg-card"
                  >
                    <div>
                      <span className="text-[10px] tracking-[0.2em] uppercase text-text-sub block">
                        {item.service.name}
                      </span>
                      {item.quantity > 1 && (
                        <span className="text-[9px] text-text-sub">
                          x{item.quantity}
                        </span>
                      )}
                    </div>
                    <span className="text-sm text-primary font-medium">
                      ${item.service.price * item.quantity}
                    </span>
                  </div>
                ))}

                <div className="flex justify-between items-center pt-2">
                  <span className="text-[10px] tracking-[0.2em] uppercase text-text-main font-medium">
                    Total
                  </span>
                  <div className="text-right">
                    <p className="font-serif text-3xl text-primary">
                      ${grandTotal}
                    </p>
                    <p className="text-[10px] text-text-sub tracking-widest uppercase">
                      USD
                    </p>
                  </div>
                </div>
              </div>

              {error && (
                <div className="border-l-2 border-red-400 pl-3 py-2 mb-4">
                  <p className="text-red-500 text-xs">{error}</p>
                </div>
              )}

              <div className="space-y-3">
                <button
                  onClick={handleContinue}
                  disabled={saving}
                  className="btn-primary w-full py-4 text-xs"
                >
                  {saving
                    ? "Guardando..."
                    : selected.length > 0
                    ? "Continuar al pago"
                    : "Continuar sin servicios"}
                </button>
                <button
                  onClick={handleSkip}
                  className="w-full py-3 text-[10px] tracking-[0.2em] uppercase text-text-sub hover:text-primary transition-colors font-semibold"
                >
                  Omitir servicios
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}