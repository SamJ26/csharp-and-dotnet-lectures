using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHost;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            })
            .ConfigureServices(serviceCollection =>
            {
                // serviceCollection.AddHostedService<>();
            })
            .Build();


        await host.RunAsync();
    }
}
