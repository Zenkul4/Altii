using Alti.Domain.Interfaces;
using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Services.Interfaces;
using Alti.Domain.Exceptions;
using Application.DTOs.Season;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class SeasonService : ISeasonService
{
    private readonly IUnitOfWork _uow;
    private readonly ISeasonFactory _factory;
    private readonly ISeasonDomainService _domainService;

    public SeasonService(IUnitOfWork uow, ISeasonFactory factory, ISeasonDomainService domainService)
    {
        _uow = uow;
        _factory = factory;
        _domainService = domainService;
    }

    public async Task<SeasonResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var season = await _uow.Seasons.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Season {id} not found.");

        return MapToResponse(season);
    }

    public async Task<IReadOnlyList<SeasonResponseDto>> GetAllAsync(CancellationToken ct = default)
    {
        var seasons = await _uow.Seasons.GetAllAsync(ct);
        return seasons.Select(MapToResponse).ToList();
    }

    public async Task<SeasonResponseDto> CreateAsync(CreateSeasonDto dto, int createdById, CancellationToken ct = default)
    {
        if (await _uow.Seasons.HasOverlapAsync(dto.StartDate, dto.EndDate, null, ct))
            throw new SeasonOverlapException(dto.StartDate, dto.EndDate, "existing season");

        var season = _factory.Create(dto.Name, dto.StartDate, dto.EndDate, dto.Multiplier, createdById, dto.Description);

        await _uow.Seasons.AddAsync(season, ct);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(season);
    }

    public async Task<SeasonResponseDto> UpdateAsync(int id, UpdateSeasonDto dto, CancellationToken ct = default)
    {
        var season = await _uow.Seasons.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Season {id} not found.");

        if (await _uow.Seasons.HasOverlapAsync(dto.StartDate, dto.EndDate, id, ct))
            throw new SeasonOverlapException(dto.StartDate, dto.EndDate, "existing season");

        _domainService.Update(season, dto.Name, dto.StartDate, dto.EndDate, dto.Multiplier, dto.Description);
        _uow.Seasons.Update(season);
        await _uow.SaveChangesAsync(ct);

        return MapToResponse(season);
    }

    private static SeasonResponseDto MapToResponse(Alti.Domain.Entities.Season season) => new()
    {
        Id = season.Id,
        Name = season.Name,
        StartDate = season.StartDate,
        EndDate = season.EndDate,
        Multiplier = season.Multiplier,
        Description = season.Description,
        CreatedById = season.CreatedById,
        CreatedAt = season.CreatedAt
    };
}