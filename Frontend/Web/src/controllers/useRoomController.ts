import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import RoomService from "../services/RoomService";
import type { RoomResponseDto } from "../models/Room";

const useRoomController = () => {
  const { user }    = useAuth();
  const navigate    = useNavigate();
  const [rooms,     setRooms]    = useState<RoomResponseDto[]>([]);
  const [filtered,  setFiltered] = useState<RoomResponseDto[]>([]);
  const [loading,   setLoading]  = useState(false);
  const [searched,  setSearched] = useState(false);
  const [error,     setError]    = useState("");
  const [typeFilter, setTypeFilter] = useState<number | undefined>(undefined);

  const searchRooms = async (
    checkIn: string,
    checkOut: string,
    type?: number,
    minCapacity?: number
  ) => {
    setError("");
    setLoading(true);
    setTypeFilter(undefined);
    try {
      const result = await RoomService.getAvailable(checkIn, checkOut, type, minCapacity);
      setRooms(result);
      setFiltered(result);
      setSearched(true);
    } catch (err: any) {
      setError(err.message ?? "Error al buscar habitaciones.");
    } finally {
      setLoading(false);
    }
  };

  const filterByType = (type: number | undefined) => {
    setTypeFilter(type);
    if (type === undefined) {
      setFiltered(rooms);
    } else {
      setFiltered(rooms.filter(r => r.type === type));
    }
  };

  const selectRoom = (room: RoomResponseDto, checkIn: string, checkOut: string) => {
    if (!user) { navigate("/login"); return; }
    navigate("/bookings/new", { state: { room, checkIn, checkOut } });
  };

  return {
    rooms: filtered,
    allRooms: rooms,
    loading,
    searched,
    error,
    typeFilter,
    searchRooms,
    filterByType,
    selectRoom,
  };
};

export default useRoomController;