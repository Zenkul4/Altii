using Infrastructure.Logging.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging.Implementations;

public class LoggerService : ILoggerService
{
    private readonly ILogger<LoggerService> _logger;

    public LoggerService(ILogger<LoggerService> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
        => _logger.LogInformation("{Message}", message);

    public void LogWarning(string message)
        => _logger.LogWarning("{Message}", message);

    public void LogError(string message, Exception? exception = null)
    {
        if (exception is null)
            _logger.LogError("{Message}", message);
        else
            _logger.LogError(exception, "{Message}", message);
    }
}