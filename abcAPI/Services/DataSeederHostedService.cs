using abcAPI.Models;

namespace abcAPI.Services;

public class DataSeederHostedService: IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DataSeederHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        DataSeeder.SeedData(scope.ServiceProvider);
        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}