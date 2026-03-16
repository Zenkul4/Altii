using Alti.Domain.Entities;
using Application.DTOs.Season;

namespace Application.Mappers;

public static class SeasonMapper
{
    public static SeasonResponseDto ToDto(Season season) => new()
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