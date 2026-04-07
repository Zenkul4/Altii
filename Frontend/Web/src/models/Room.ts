export interface RoomResponseDto {
  id: number;
  number: string;
  type: number;
  floor: number;
  capacity: number;
  basePrice: number;
  description?: string;
  status: number;
}

export const RoomTypeLabel: Record<number, string> = {
  0: "Single",
  1: "Double",
  2: "Suite",
  3: "Family",
  4: "Penthouse",
};