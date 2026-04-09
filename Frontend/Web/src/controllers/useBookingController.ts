import { useState, useEffect } from "react";
import { useAuth } from "../context/AuthContext";
import { useToast } from "../context/ToastContext";
import BookingService from "../services/BookingService";
import type { BookingResponseDto, CreateBookingDto } from "../models/Booking";
import type { CreateBookingByTypeDto } from "../services/BookingService";
import { useNotifications } from "../context/NotificationContext";

const useBookingController = () => {
    const { user } = useAuth();
    const { showToast } = useToast();
    const [bookings, setBookings] = useState<BookingResponseDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const { addNotification } = useNotifications();

    const loadMyBookings = async () => {
        if (!user) return;
        setLoading(true);
        try {
            const result = await BookingService.getByGuest(user.id);
            setBookings(result);
        } catch (err: unknown) {
            const message = err instanceof Error ? err.message : "Error al cargar reservas.";
            setError(message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => { loadMyBookings(); }, [user]);

    const createBooking = async (dto: CreateBookingDto) => {
        setError("");
        setLoading(true);
        try {
            const booking = await BookingService.create(dto);
            addNotification("booking", "Reserva creada", `Reserva ${booking.code} creada. Complete el pago para confirmarla.`);
            return booking;
        } catch (err: unknown) {
            const message = err instanceof Error ? err.message : "Error al crear la reserva.";
            setError(message);
            return null;
        } finally {
            setLoading(false);
        }
    };

    const createBookingByType = async (dto: CreateBookingByTypeDto) => {
        setError("");
        setLoading(true);
        try {
            const booking = await BookingService.createByType(dto);
            addNotification("booking", "Reserva creada", `Reserva ${booking.code} creada. Complete el pago para confirmarla.`);
            return booking;
        } catch (err: unknown) {
            const message = err instanceof Error ? err.message : "Error al crear la reserva.";
            setError(message);
            return null;
        } finally {
            setLoading(false);
        }
    };

    const cancelBooking = async (id: number) => {
        if (!confirm("¿Está seguro de cancelar esta reserva?")) return;
        try {
            await BookingService.cancel(id);
            setBookings(prev => prev.map(b => b.id === id ? { ...b, status: 4 } : b));
            showToast("Reserva cancelada correctamente", "success");
            addNotification("cancel", "Reserva cancelada", `La reserva fue cancelada exitosamente.`);
        } catch (err: unknown) {
            const message = err instanceof Error ? err.message : "No se pudo cancelar.";
            showToast(message, "error");
        }
    };

    return { bookings, loading, error, createBooking, createBookingByType, cancelBooking, loadMyBookings };
};

export default useBookingController;