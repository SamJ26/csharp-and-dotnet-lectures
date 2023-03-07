namespace Worker;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                // These services will be running in the background
                services.AddHostedService<WorkerA>();
                services.AddHostedService<WorkerB>();

            })
            .Build();
        
        host.Run();
    }
}