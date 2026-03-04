using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services.Implementations;

public class CodeGeneratorService : ICodeGeneratorService
{
    private static int _sequence = 0;
    private static readonly Lock _lock = new();

    public string GenerateBookingCode()
    {
        int next;

        lock (_lock)
        {
            next = ++_sequence;
        }

        return $"RES-{DateTime.UtcNow:yyyy}-{next:D5}";
    }
}