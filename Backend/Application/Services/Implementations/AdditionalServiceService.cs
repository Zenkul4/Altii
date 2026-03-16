using Alti.Domain.Factories.Interfaces;
using Alti.Domain.Interfaces;
using Alti.Domain.Services.Interfaces;
using Application.Dtos.AdditionalService;
using Application.Mappers;
using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class AdditionalServiceService : IAdditionalServiceService
{
    private readonly IUnitOfWork _uow;
    private readonly IAdditionalServiceFactory _factory;
    private readonly IAdditionalServiceDomainService _domainService;

    public AdditionalServiceService(
        IUnitOfWork uow,
        IAdditionalServiceFactory factory,
        IAdditionalServiceDomainService domainService)
    {
        _uow = uow;
        _factory = factory;
        _domainService = domainService;
    }

    public async Task<AdditionalServiceResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var service = await _uow.AdditionalServices.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Service {id} not found.");

        return AdditionalServiceMapper.ToDto(service);
    }

    public async Task<IReadOnlyList<AdditionalServiceResponseDto>> GetAllActiveAsync(CancellationToken ct = default)
    {
        var services = await _uow.AdditionalServices.GetAllActiveAsync(ct);
        return services.Select(AdditionalServiceMapper.ToDto).ToList();
    }

    public async Task<AdditionalServiceResponseDto> CreateAsync(CreateAdditionalServiceDto dto, CancellationToken ct = default)
    {
        var service = _factory.Create(dto.Name, dto.Price, dto.Description);

        await _uow.AdditionalServices.AddAsync(service, ct);
        await _uow.SaveChangesAsync(ct);

        return AdditionalServiceMapper.ToDto(service);
    }

    public async Task<AdditionalServiceResponseDto> UpdateAsync(int id, UpdateAdditionalServiceDto dto, CancellationToken ct = default)
    {
        var service = await _uow.AdditionalServices.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Service {id} not found.");

        _domainService.UpdateDetails(service, dto.Price, dto.Description);
        _uow.AdditionalServices.Update(service);
        await _uow.SaveChangesAsync(ct);

        return AdditionalServiceMapper.ToDto(service);
    }

    public async Task DeactivateAsync(int id, CancellationToken ct = default)
    {
        var service = await _uow.AdditionalServices.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Service {id} not found.");

        _domainService.Deactivate(service);
        _uow.AdditionalServices.Update(service);
        await _uow.SaveChangesAsync(ct);
    }

    public async Task ActivateAsync(int id, CancellationToken ct = default)
    {
        var service = await _uow.AdditionalServices.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Service {id} not found.");

        _domainService.Activate(service);
        _uow.AdditionalServices.Update(service);
        await _uow.SaveChangesAsync(ct);
    }
}