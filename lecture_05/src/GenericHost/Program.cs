using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHost;

class Program
{
    static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureHostConfiguration(configBuilder =>
            {
                configBuilder.AddJsonFile("host-config.json");
            })
            .ConfigureAppConfiguration(configBuilder =>
            {
                configBuilder.AddJsonFile("app-config.json");

            })
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            })
            .ConfigureServices(serviceCollection =>
            {
                // These services will be running in the background
                serviceCollection.AddHostedService<ServiceA>();
                serviceCollection.AddHostedService<ServiceB>();
                
            })
            .Build();
        
        host.Run();
    }
}
