using Alti.Domain.Enums;
using Alti.Domain.Interfaces;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Services.Interfaces;
using Application.DTOs.Room;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _uow;
    private readonly IRoomFactory _factory;
    private readonly IRoomDomainService _domainService;

    public RoomService(IUnitOfWork uow, IRoomFactory factory, IRoomDomainService domainService)
    {
        _uow = uow;
        _factory = factory;
        _domainService = domainService;
    }

    public async Task<RoomResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var room = await _uow.Rooms.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Room {id} not found.");

        return MapToResponse(room);
    }

    public async Task<IReadOnlyList<RoomResponseDto>> GetAvailableAsync(
        DateOnly checkIn,
        DateOnly checkOut,
        RoomType? type = null,
        short? minCapacity = null,
        CancellationToken ct = default)
    {
        var rooms = await _uow.Rooms.GetAvailableAsync(checkIn, checkOut, type, minCapacity, ct);
        return rooms.Select(MapToResponse).ToList();
    }

    public async Task<RoomResponseDto> CreateAsync(CreateRoomDto dto, CancellationToken ct = default)
    {
        var room = _factory.Create(dto.Number, dto.Type, dto.Floor, dto.Capacity, dto.BasePrice, dto.Description);

        await _uow.Rooms.AddAsync(room, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(room);
    }

    public async Task<RoomResponseDto> UpdateAsync(int id, UpdateRoomDto dto, CancellationToken ct = default)
    {
        var room = await _uow.Rooms.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Room {id} not found.");

        _domainService.UpdateDetails(room, dto.BasePrice, dto.Description);
        _uow.Rooms.Update(room);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(room);
    }

    public async Task DisableAsync(int id, CancellationToken ct = default)
    {
        var room = await _uow.Rooms.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Room {id} not found.");

        _domainService.Disable(room);
        _uow.Rooms.Update(room);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task EnableAsync(int id, CancellationToken ct = default)
    {
        var room = await _uow.Rooms.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Room {id} not found.");

        _domainService.Enable(room);
        _uow.Rooms.Update(room);
        await _uow.SaveChangesAsync(ct);
    }

    private static RoomResponseDto MapToResponse(Alti.Domain.Entities.Room room) => new()
    {
        Id = room.Id,
        Number = room.Number,
        Type = room.Type,
        Floor = room.Floor,
        Capacity = room.Capacity,
        BasePrice = room.BasePrice,
        Description = room.Description,
        Status = room.Status,
        CreatedAt = room.CreatedAt
    };
}