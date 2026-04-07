import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import usePaymentController from "../../controllers/usePaymentController";
import type { BookingResponseDto } from "../../models/Booking";

const methods = [
    { value: "CreditCard", label: "Tarjeta de Crédito", icon: "💳" },
    { value: "DebitCard", label: "Tarjeta de Débito", icon: "💳" },
    { value: "Cash", label: "Efectivo en recepción", icon: "💵" },
    { value: "BankTransfer", label: "Transferencia Bancaria", icon: "🏦" },
];

export default function PaymentPage() {
    const navigate = useNavigate();
    const { state } = useLocation() as { state: { booking: BookingResponseDto; total: number } };
    const { loading, error, processPayment } = usePaymentController();
    const [method, setMethod] = useState("CreditCard");

    if (!state?.booking) { navigate("/my-bookings"); return null; }
    const { booking, total } = state;

    return (
        <div className="min-h-screen bg-bg-base pt-20">
            <div className="max-w-5xl mx-auto px-6 py-16">

                <button onClick={() => navigate(-1)}
                    className="text-[10px] tracking-[0.2em] uppercase text-text-sub hover:text-primary transition-colors flex items-center gap-2 mb-10">
                    ← Volver
                </button>

                <div className="flex items-center gap-6 mb-12">
                    <div className="h-px flex-1 bg-bg-card" />
                    <span className="text-primary text-[10px] tracking-[0.4em] uppercase">Procesar Pago</span>
                    <div className="h-px flex-1 bg-bg-card" />
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-2 gap-10">

                    {/* Method */}
                    <div className="bg-bg-white border border-bg-card p-8">
                        <h2 className="font-serif text-2xl text-text-main mb-2">Método de pago</h2>
                        <p className="text-text-sub text-xs tracking-wide mb-8">Seleccione su forma de pago preferida</p>
                        <div className="space-y-3">
                            {methods.map(m => (
                                <label key={m.value}
                                    className={`flex items-center gap-4 p-4 border cursor-pointer transition-all duration-200 ${method === m.value ? "border-primary bg-primary/5" : "border-bg-card hover:border-primary/50"
                                        }`}>
                                    <input type="radio" name="method" value={m.value}
                                        checked={method === m.value} onChange={() => setMethod(m.value)}
                                        className="accent-primary" />
                                    <span className="text-xl">{m.icon}</span>
                                    <span className="text-sm text-text-main font-medium">{m.label}</span>
                                    {method === m.value && <span className="ml-auto text-primary text-lg">✓</span>}
                                </label>
                            ))}
                        </div>
                        <div className="mt-8 bg-bg-base p-4">
                            <p className="text-[10px] tracking-wide text-text-sub leading-relaxed">
                                ✦ Pagos seguros con cifrado SSL de 256 bits<br />
                                ✦ No almacenamos datos de tarjetas<br />
                                ✦ Recibo enviado automáticamente por email
                            </p>
                        </div>
                    </div>

                    {/* Summary */}
                    <div className="flex flex-col gap-6">
                        <div className="bg-bg-white border border-bg-card p-8">
                            <h2 className="font-serif text-2xl text-text-main mb-8">Resumen del pago</h2>
                            <div className="space-y-4">
                                {[
                                    { label: "Reserva", value: booking.code },
                                    { label: "Check-In", value: booking.checkInDate },
                                    { label: "Check-Out", value: booking.checkOutDate },
                                    { label: "Método", value: methods.find(m => m.value === method)?.label ?? "" },
                                ].map(row => (
                                    <div key={row.label} className="flex justify-between pb-4 border-b border-bg-card">
                                        <span className="text-[10px] tracking-[0.2em] uppercase text-text-sub">{row.label}</span>
                                        <span className="text-sm text-text-main">{row.value}</span>
                                    </div>
                                ))}
                                <div className="flex justify-between items-center pt-2">
                                    <span className="text-[10px] tracking-[0.2em] uppercase text-text-main font-medium">Total a pagar</span>
                                    <div className="text-right">
                                        <p className="font-serif text-4xl text-primary">${total}</p>
                                        <p className="text-[10px] text-text-sub tracking-widest uppercase">USD</p>
                                    </div>
                                </div>
                            </div>
                        </div>

                        {error && (
                            <div className="border-l-2 border-red-400 pl-4 py-3">
                                <p className="text-red-500 text-xs tracking-wide">{error}</p>
                            </div>
                        )}

                        <div className="flex gap-4">
                            <button onClick={() => navigate(-1)} className="btn-outline flex-1 py-4">Cancelar</button>
                            <button
                                onClick={() => processPayment(
                                    { bookingId: booking.id, amount: total, paymentMethod: method },
                                    booking   // ← pasa el booking
                                )}
                                disabled={loading}
                                className="btn-primary flex-1 py-4">
                                {loading ? "Procesando..." : "Confirmar pago"}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}