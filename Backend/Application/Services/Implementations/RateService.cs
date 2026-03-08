using Alti.Domain.Interfaces;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Services.Interfaces;
using Application.DTOs.Rate;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class RateService : IRateService
{
    private readonly IUnitOfWork _uow;
    private readonly IRateFactory _factory;
    private readonly IRateDomainService _domainService;

    public RateService(IUnitOfWork uow, IRateFactory factory, IRateDomainService domainService)
    {
        _uow = uow;
        _factory = factory;
        _domainService = domainService;
    }

    public async Task<RateResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var rate = await _uow.Rates.GetBySeasonAndTypeAsync(id, default, ct)
            ?? throw new KeyNotFoundException($"Rate {id} not found.");

        return MapToResponse(rate);
    }

    public async Task<IReadOnlyList<RateResponseDto>> GetBySeasonAsync(int seasonId, CancellationToken ct = default)
    {
        var rates = await _uow.Rates.GetBySeasonAsync(seasonId, ct);
        return rates.Select(MapToResponse).ToList();
    }

    public async Task<RateResponseDto> CreateAsync(CreateRateDto dto, int createdById, CancellationToken ct = default)
    {
        var rate = _factory.Create(dto.SeasonId, dto.RoomType, dto.PricePerNight, createdById);

        await _uow.Rates.AddAsync(rate, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(rate);
    }

    public async Task<RateResponseDto> UpdateAsync(int id, UpdateRateDto dto, CancellationToken ct = default)
    {
        var rate = await _uow.Rates.GetBySeasonAndTypeAsync(id, default, ct)
            ?? throw new KeyNotFoundException($"Rate {id} not found.");

        _domainService.UpdatePrice(rate, dto.PricePerNight);
        _uow.Rates.Update(rate);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(rate);
    }

    private static RateResponseDto MapToResponse(Alti.Domain.Entities.Rate rate) => new()
    {
        Id = rate.Id,
        SeasonId = rate.SeasonId,
        RoomType = rate.RoomType,
        PricePerNight = rate.PricePerNight,
        CreatedById = rate.CreatedById,
        CreatedAt = rate.CreatedAt
    };
}