using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using WebApplication1.Models;
using WebApplication1.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;


namespace WebApplication1.Services
{
    
    public class SureKontrolIscisi : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<AracHub> _hubContext;

        public SureKontrolIscisi(IServiceProvider serviceProvider, IHubContext<AracHub> hubContext)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) {
                using (var scope = _serviceProvider.CreateScope()) {
                    var context= scope.ServiceProvider.GetRequiredService<dbContextClass>();
                    var iceridekiAraclar = context.visitorLog.Where(a=>a.cikisZamani==null).ToList();
                    bool degisiklikVarMi = false;

                    foreach (var arac in iceridekiAraclar)
                    {
                        TimeSpan gecenSure = DateTime.Now - arac.girisZamani;

                        
                        if (gecenSure.TotalMinutes > 240 && !arac.asimVarMi)
                        {
                            arac.asimVarMi = true;
                            degisiklikVarMi = true;

                        
                            await _hubContext.Clients.All.SendAsync("AracAsimYapti", arac.id);
                        }
                    }

                    
                    if (degisiklikVarMi)
                    {
                        context.SaveChanges();
                    }
                }

               
                await Task.Delay(120000, stoppingToken);
            }
        }
    }
}


