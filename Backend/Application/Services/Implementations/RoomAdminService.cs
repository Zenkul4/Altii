using Alti.Domain.Interfaces;
using Alti.Domain.Interfaces.Repositories;
using Alti.Domain.Services.Interfaces;
using Application.DTOs.Room;
using Application.Interfaces;
using Application.Mappers;

namespace Application.Services;

public class RoomAdminService : IRoomAdminService
{
    private readonly IRoomAdminRepository _roomAdminRepository;
    private readonly IRoomDomainService _roomDomainService;
    private readonly IUnitOfWork _unitOfWork;

    public RoomAdminService(
        IRoomAdminRepository roomAdminRepository,
        IRoomDomainService roomDomainService,
        IUnitOfWork unitOfWork)
    {
        _roomAdminRepository = roomAdminRepository;
        _roomDomainService = roomDomainService;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoomResponseDto>> GetAllRoomsAsync()
    {
        var rooms = await _roomAdminRepository.GetAllAsync();
        return rooms.Select(RoomMapper.ToDto);
    }

    public async Task MarkOccupiedAsync(int id)
    {
        var room = await _roomAdminRepository.GetByIdAsync(id);
        if (room is null) throw new KeyNotFoundException($"Habitación {id} no encontrada.");
        _roomDomainService.MarkAsOccupied(room);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task MarkCleaningAsync(int id)
    {
        var room = await _roomAdminRepository.GetByIdAsync(id);
        if (room is null) throw new KeyNotFoundException($"Habitación {id} no encontrada.");
        _roomDomainService.SendToCleaning(room);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task MarkBlockedAsync(int id)
    {
        var room = await _roomAdminRepository.GetByIdAsync(id);
        if (room is null) throw new KeyNotFoundException($"Habitación {id} no encontrada.");
        _roomDomainService.Block(room);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ReleaseBlockAsync(int id)
    {
        var room = await _roomAdminRepository.GetByIdAsync(id);
        if (room is null) throw new KeyNotFoundException($"Habitación {id} no encontrada.");
        _roomDomainService.ReleaseBlock(room);
        await _unitOfWork.SaveChangesAsync();
    }
}