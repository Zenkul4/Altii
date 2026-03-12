using Alti.Domain.Entities;
using Application.Dtos.AdditionalService;

namespace Application.Mappers;

public static class AdditionalServiceMapper
{
    public static AdditionalServiceResponseDto ToDto(AdditionalService service) => new()
    {
        Id = service.Id,
        Name = service.Name,
        Description = service.Description,
        Price = service.Price,
        IsActive = service.IsActive
    };
}