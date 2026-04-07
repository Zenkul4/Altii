using Application.Interfaces;

namespace Infrastructure.Services.Implementations;

public class CodeGeneratorService : ICodeGeneratorService
{
    public string GenerateBookingCode()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = Random.Shared.Next(1000, 9999);
        return $"RES-{DateTime.UtcNow:yyyy}-{timestamp[8..]}{random}";
    }
}