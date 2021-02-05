using AngularApi.DataBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace angularapi.MyTools
{
    public class BackgroundTask 
    {
        private readonly ILogger<BackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        public BackgroundTask(ILogger<BackgroundService> logger,
                 IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

 
        public async Task RemoveUnverifiedUserAsync(string randomToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<CashDBContext>();

                await Task.Delay(TimeSpan.FromMinutes(10));
                try
                {
                    var user = _context.userDBModels.FirstOrDefault(s => s.VeryficationToken == randomToken);
                    if (user != null && user.IsVerify == false)
                    {
                        _context.userDBModels.Remove(user);
                        _context.SaveChanges();
                        await Task.CompletedTask;
                        _logger.LogInformation("Removed {0} from database", user.Name);
                    }
                }
                catch (Exception e)
                {
                    throw new ApplicationException(e.Message);
                }
            }
        }

      
    }
}
