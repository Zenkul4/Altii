import client from "./client";
import type { PaymentResponseDto, CreatePaymentDto } from "../models/Payment";

const PaymentService = {
  create: async (dto: CreatePaymentDto): Promise<PaymentResponseDto> => {
    const { data } = await client.post("/Payments", dto);
    return data;
  },

  getByBooking: async (bookingId: number): Promise<PaymentResponseDto[]> => {
    const { data } = await client.get(`/Payments/booking/${bookingId}`);
    return data;
  },
};

export default PaymentService;