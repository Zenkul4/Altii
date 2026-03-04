using Alti.Domain.Entities;

namespace Alti.Domain.Services.Interfaces;

public interface IRoomDomainService
{
    void Block(Room room);
    void ReleaseBlock(Room room);
    void MarkAsOccupied(Room room);
    void SendToCleaning(Room room);
    void MarkAsAvailable(Room room);
    void Disable(Room room);
    void Enable(Room room);
    void UpdateDetails(Room room, decimal newBasePrice, string? newDescription);
}