using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleansAsyncEnumExceptionRepro;

using var host = new HostBuilder()
    .UseOrleans(builder =>
    {
        builder.UseLocalhostClustering();
        builder.UseDashboard(dashboard =>
        {
            dashboard.Port = 9999;
        });
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .Build();

await host.StartAsync();


var grainFactory = host.Services.GetRequiredService<IGrainFactory>();
var grain = grainFactory.GetGrain<ITest>("1");

// Call the grain that will throw exception
Console.WriteLine("Enumerating ...");
await foreach (var s in grain.List())
{
    Console.WriteLine(s);
}

// We never reach here
Console.WriteLine("Press Enter to terminate...");
Console.ReadLine();
Console.WriteLine("Orleans is stopping...");

await host.StopAsync();