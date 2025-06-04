using ShoeStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using static System.Formats.Asn1.AsnWriter;

namespace ShoeStore.Services.Implementations
{
    public class InactiveCartService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

        public InactiveCartService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory; 
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    Console.WriteLine("Checking for inactive carts...");
                    var cartService = scope.ServiceProvider.GetRequiredService<ICartService>();
                    await cartService.ProcessCartActivityAsync();

                    
                    await Task.Delay(_checkInterval, stoppingToken);
                }
            }
        }
    }
}
