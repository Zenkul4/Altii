import client from "./client";
import type { RoomResponseDto } from "../models/Room";

const RoomService = {
  getAvailable: async (
    checkIn: string,
    checkOut: string,
    type?: number,
    minCapacity?: number
  ): Promise<RoomResponseDto[]> => {
    const params: Record<string, string | number> = { checkIn, checkOut };
    if (type !== undefined) params.type = type;
    if (minCapacity !== undefined) params.minCapacity = minCapacity;
    const { data } = await client.get("/Rooms/available", { params });
    return data;
  },

  getById: async (id: number): Promise<RoomResponseDto> => {
    const { data } = await client.get(`/Rooms/${id}`);
    return data;
  },
};

export default RoomService;