import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useToast } from "../context/ToastContext";
import { useNotifications } from "../context/NotificationContext";
import PaymentService from "../services/PaymentService";
import type { CreatePaymentDto } from "../models/Payment";
import type { BookingResponseDto } from "../models/Booking";

const usePaymentController = () => {
  const navigate          = useNavigate();
  const { showToast }     = useToast();
  const { addNotification } = useNotifications();
  const [loading, setLoading] = useState(false);
  const [error,   setError]   = useState("");

  const processPayment = async (
    dto:     CreatePaymentDto,
    booking: BookingResponseDto   // ← parámetro agregado
  ) => {
    setError("");
    setLoading(true);
    try {
      await PaymentService.create(dto);
      showToast("Pago registrado correctamente", "success");
      addNotification(
        "payment",
        "Pago registrado",
        `Pago de $${dto.amount} registrado. Pendiente de aprobación.`
      );
      navigate("/booking/confirmation", {
        state: { booking, total: dto.amount }  // ← usa el booking recibido
      });
    } catch (err: any) {
      setError(err.message ?? "Error al procesar el pago.");
    } finally {
      setLoading(false);
    }
  };

  return { loading, error, processPayment };
};

export default usePaymentController;