using AngularApi.Repository;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace AngularApi.MyTools
{
    public class UpdateFileService: IHostedService
    {

      private DayOfWeek day { get;  set; }

        private static Timer updateDataBase;




        public void UpdateCash(object state)
        {
            if (CheckIsItWeekend())
            {

            }

            string path = @"Data/test.json";
            path = Path.GetFullPath(path);
            string fileContent = File.ReadAllText(path);
            if (fileContent==String.Empty)
            {
                File.WriteAllText(path, "XD");
            }
            else
            {
                fileContent = File.ReadAllText(path);
                fileContent += "\nXD";
                File.WriteAllText(path, fileContent);
            }

        }

        private bool CheckIsItWeekend()
        {
            if(DateTime.Today.DayOfWeek.Equals("Sunday")|| DateTime.Today.DayOfWeek.Equals( "Saturday"))
            {
                return true;
            }
           
            return false;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            updateDataBase = new Timer(UpdateCash, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            updateDataBase?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
