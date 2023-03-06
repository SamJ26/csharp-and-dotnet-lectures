using Microsoft.Extensions.Logging;

namespace GenericHost;

public class Service
{
    private readonly ILogger<Service> _logger;

    public Service(ILogger<Service> logger)
    {
        _logger = logger;
    }

    public void Foo()
    {
        _logger.LogInformation("Information");
    }
}