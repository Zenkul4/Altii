import client from "./client";
import type { AdditionalServiceDto, BookingServiceResponseDto, CreateBookingServiceDto } from "../models/AdditionalService";

const AdditionalServiceService = {
  getAllActive: async (): Promise<AdditionalServiceDto[]> => {
    const { data } = await client.get("/AdditionalServices");
    return data;
  },

  getByBooking: async (bookingId: number): Promise<BookingServiceResponseDto[]> => {
    const { data } = await client.get(`/BookingServices/booking/${bookingId}`);
    return data;
  },

  addToBooking: async (dto: CreateBookingServiceDto): Promise<BookingServiceResponseDto> => {
    const { data } = await client.post("/BookingServices", dto);
    return data;
  },
};

export default AdditionalServiceService;