export interface AdditionalServiceDto {
  id: number;
  name: string;
  description?: string;
  price: number;
  isActive: boolean;
}

export interface BookingServiceResponseDto {
  id: number;
  bookingId: number;
  serviceId: number;
  serviceName: string;
  quantity: number;
  unitPrice: number;
  subtotal: number;
  registeredAt: string;
}

export interface CreateBookingServiceDto {
  bookingId: number;
  serviceId: number;
  registeredById: number;
  quantity: number;
}