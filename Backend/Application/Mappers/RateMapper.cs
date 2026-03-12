using Alti.Domain.Entities;
using Application.Dtos.Rate;

namespace Application.Mappers;

public static class RateMapper
{
    public static RateResponseDto ToDto(Rate rate) => new()
    {
        Id = rate.Id,
        SeasonId = rate.SeasonId,
        RoomType = rate.RoomType,
        PricePerNight = rate.PricePerNight,
        CreatedById = rate.CreatedById,
        CreatedAt = rate.CreatedAt
    };
}