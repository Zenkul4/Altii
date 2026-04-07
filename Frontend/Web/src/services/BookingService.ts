import client from "./client";
import type { BookingResponseDto, CreateBookingDto } from "../models/Booking";

const BookingService = {
  create: async (dto: CreateBookingDto): Promise<BookingResponseDto> => {
    const { data } = await client.post("/Bookings", dto);
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
};

export default BookingService;