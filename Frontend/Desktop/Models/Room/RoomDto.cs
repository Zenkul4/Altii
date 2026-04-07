namespace Desktop.Models.Room;

public class RoomDto
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public int Type { get; set; }
    public short Floor { get; set; }
    public short Capacity { get; set; }
    public decimal BasePrice { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }

    public string TypeLabel
    {
        get => Type switch { 0 => "Single", 1 => "Doble", 2 => "Suite", 3 => "Familiar", 4 => "Penthouse", _ => $"Tipo {Type}" };
        private set { }
    }

    public string StatusLabel
    {
        get => Status switch { 0 => "Disponible", 1 => "Ocupada", 2 => "Limpieza", 3 => "Bloqueada", 4 => "Inactiva", _ => $"Estado {Status}" };
        private set { }
    }

    public string StatusColor
    {
        get => Status switch { 0 => "#10B981", 1 => "#3B82F6", 2 => "#F59E0B", 3 => "#6B7280", 4 => "#EF4444", _ => "#6B7280" };
        private set { }
    }

    public class CreateRoomDto
    {
        public string Number { get; set; } = string.Empty;
        public int Type { get; set; } = 0;
        public short Floor { get; set; }
        public short Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateRoomDto
    {
        public decimal BasePrice { get; set; }
        public string? Description { get; set; }
    }
}