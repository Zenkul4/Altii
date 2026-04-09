import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import RoomService from "../services/RoomService";
import type { RoomTypeAvailabilityDto } from "../models/Room";

const useRoomController = () => {
  const { user } = useAuth();
  const navigate = useNavigate();
  const [availability, setAvailability] = useState<RoomTypeAvailabilityDto[]>([]);
  const [filtered, setFiltered] = useState<RoomTypeAvailabilityDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [searched, setSearched] = useState(false);
  const [error, setError] = useState("");
  const [typeFilter, setTypeFilter] = useState<number | undefined>(undefined);

  const searchRooms = async (checkIn: string, checkOut: string) => {
    setError("");
    setLoading(true);
    setTypeFilter(undefined);
    try {
      const result = await RoomService.getAvailabilityByType(checkIn, checkOut);
      setAvailability(result);
      setFiltered(result);
      setSearched(true);
    } catch (err: unknown) {
  const message = err instanceof Error ? err.message : "Error al buscar habitaciones.";
  setError(message);
    } finally {
      setLoading(false);
    }
  };

  const filterByType = (type: number | undefined) => {
    setTypeFilter(type);
    if (type === undefined) {
      setFiltered(availability);
    } else {
      setFiltered(availability.filter((r) => r.type === type));
    }
  };

  const selectType = (
    roomType: RoomTypeAvailabilityDto,
    checkIn: string,
    checkOut: string
  ) => {
    if (!user) {
      navigate("/login");
      return;
    }
    navigate("/bookings/new", { state: { roomType, checkIn, checkOut } });
  };

  return {
    rooms: filtered,
    allRooms: availability,
    loading,
    searched,
    error,
    typeFilter,
    searchRooms,
    filterByType,
    selectType,
  };
};

export default useRoomController;