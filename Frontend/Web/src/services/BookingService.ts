import client from "./client";
import type { BookingResponseDto, CreateBookingDto } from "../models/Booking";

export interface CreateBookingByTypeDto {
  guestId: number;
  roomType: number;
  checkInDate: string;
  checkOutDate: string;
  notes?: string;
}

const BookingService = {
  create: async (dto: CreateBookingDto): Promise<BookingResponseDto> => {
    const { data } = await client.post("/Bookings", dto);
    return data;
  },

  createByType: async (dto: CreateBookingByTypeDto): Promise<BookingResponseDto> => {
    const { data } = await client.post("/Bookings/by-type", dto);
    return data;
  },

  getByGuest: async (guestId: number, page = 1, pageSize = 10): Promise<BookingResponseDto[]> => {
    const { data } = await client.get(`/Bookings/guest/${guestId}`, {
      params: { page, pageSize },
    });
    return data;
  },

  getById: async (id: number): Promise<BookingResponseDto> => {
    const { data } = await client.get(`/Bookings/${id}`);
    return data;
  },

  getByCode: async (code: string): Promise<BookingResponseDto> => {
    const { data } = await client.get(`/Bookings/code/${code}`);
    return data;
  },

  cancel: async (id: number): Promise<void> => {
    await client.patch(`/Bookings/${id}/cancel`);
  },

  getExpectedTotal: async (bookingId: number): Promise<number> => {
    const { data } = await client.get(`/Bookings/${bookingId}/expected-total`);
    return data.total;
  },
};

export default BookingService;