export interface BookingResponseDto {
  id: number;
  code: string;
  guestId: number;
  roomId: number;
  roomNumber: string;
  checkInDate: string;
  checkOutDate: string;
  nights: number;
  pricePerNight: number;
  totalPrice: number;
  status: number;
  expiresAt: string;
  notes?: string;
}

export interface CreateBookingDto {
  guestId: number;
  roomId: number;
  checkInDate: string;
  checkOutDate: string;
  notes?: string;
}

export const BookingStatusLabel: Record<number, string> = {
  0: "Pendiente de Pago",
  1: "Confirmada",
  2: "Check-In",
  3: "Check-Out",
  4: "Cancelada",
  5: "Expirada",
};

export const BookingStatusColor: Record<number, string> = {
  0: "bg-yellow-100 text-yellow-800",
  1: "bg-green-100 text-green-800",
  2: "bg-blue-100 text-blue-800",
  3: "bg-gray-100 text-gray-600",
  4: "bg-red-100 text-red-700",
  5: "bg-orange-100 text-orange-700",
};