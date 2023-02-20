using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Player.API;
using Reservation.Helpers;

namespace Player.Reminders
{
    internal class PushNotificationHostedService : BackgroundService
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly TimeSpan ExecuteInterval = TimeSpan.FromMinutes(1);
        private DateTime lastRun;

        public PushNotificationHostedService(IServiceProvider sp)
        {
            ServiceProvider = sp;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await StartWorker(stoppingToken);
        }

        private async Task StartWorker(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = ServiceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<IPushNotificationService>();
                        var now = DateTime.Now;
                        if (lastRun == DateTime.MinValue)
                        {
                            lastRun = now.Add(-ExecuteInterval);
                        }
                        await service.DoWork(lastRun, now, stoppingToken);
                        lastRun = now;
                    }
                    await Task.Delay(ExecuteInterval);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex.Message, ex.StackTrace);
            }

        }

        
    }
}
