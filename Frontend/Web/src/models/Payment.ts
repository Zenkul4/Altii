export interface PaymentResponseDto {
  id: number;
  bookingId: number;
  amount: number;
  status: number;
  externalReference?: string;
  paymentMethod?: string;
  processedAt?: string;
}

export interface CreatePaymentDto {
  bookingId: number;
  amount: number;
  paymentMethod: string;
}

export const PaymentStatusLabel: Record<number, string> = {
  0: "Pendiente",
  1: "Aprobado",
  2: "Rechazado",
  3: "Reembolsado",
};