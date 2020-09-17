using AngularApi.DataBase;
using AngularApi.Models;
using AngularApi.Repository;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AngularApi.MyTools
{

    public class UpdateFileService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;

        public static string IsoPath = @"Data/Iso.json";
        public static string RecentHistoryPath = @"Data/RecentHistory.json";
        public static string TestPath = @"Data/test.json";
        public static string LogsPath = @"Data/Logs.txt";

        private static Timer updateDataBase;
        private TimeSpan periodTime;
  


        public UpdateFileService(ILogger<UpdateFileService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

        }


        List<CashDBModel> listOfCash = new List<CashDBModel>();
        string[] isoArray;

        public void UpdateCash(object state)
        {
            periodTime = SetMinuts();
            using (var scope = _scopeFactory.CreateScope())
            {
                var _update = scope.ServiceProvider.GetRequiredService<IUpdateFile>();
                var _context = scope.ServiceProvider.GetRequiredService<CashDBContext>();


                if (DateTime.Today.DayOfWeek.ToString() !=  "Saturday" &&
                       DateTime.Today.DayOfWeek.ToString() != "Sunday")
                {
                    if (ChceckItIsAvailableApi())
                    {
                        isoArray = _update.GetIsoFromFile(isoArray);
                        foreach (string iso in isoArray)
                        {
                            listOfCash.Add(_update.DownloadActual(iso));
                        }

                        _update.SendCurrencyToDataBase(listOfCash, _context);
                    }

                }
            }

        }


        private bool ChceckItIsAvailableApi()
        {
            DateTime utcNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"));
            if (utcNow.Hour>8 && utcNow.Hour < 22)
            {
                return true;
            }
            if (utcNow.Hour == 8 && utcNow.Minute > 16) 
            {
                return true;
            }
            return false;
        }



        private TimeSpan SetMinuts()
        {
            if (DateTime.UtcNow.Hour < 8)
            {
                int acctualhour = DateTime.UtcNow.Hour;
                for (int i = acctualhour; i < 24; i++)
                {
                    if (acctualhour == 8)
                    {
                        break;

                    }
                }

                return TimeSpan.FromHours(24) + TimeSpan.FromMinutes(16);
            }
            else if (DateTime.UtcNow.Hour >20)
            {
                int acctualhour = DateTime.UtcNow.Hour;
                int hour = 0;
                for (int i = acctualhour; i <= 24; i++)
                {
                    if (acctualhour == 24)
                    {
                        hour = i;
                        break;

                    }
                }
                for (int i = acctualhour; i < 24; i++)
                {
                    if (acctualhour == 8)
                    {
                        hour += i;
                        break;

                    }
                }
                return TimeSpan.FromHours(hour) + TimeSpan.FromMinutes(16);
            }
            else
            {
                int hourToSubtract = 0;
                int acctualhour = DateTime.UtcNow.Hour;
                for (int i = acctualhour; i > 0; i--)
                {
                    if (acctualhour == 8)
                    {
                        hourToSubtract = i;
                        break;
                    }
                }
                return TimeSpan.FromHours(24 - hourToSubtract) + TimeSpan.FromMinutes(16);
            }
        }
        public static void SaveToFile(string pathToFile, string text, bool appendText)
        {
            string path = Path.GetFullPath(pathToFile);
            if (appendText)
            {
                string fileContent = File.ReadAllText(path);
                if (fileContent == String.Empty)
                {
                    File.WriteAllText(path, text);
                }
                else
                {
                    fileContent = File.ReadAllText(path);
                    fileContent += "\n" + text;
                    File.WriteAllText(path, fileContent);
                }
            }
            else
            {
                File.WriteAllText(path, text);
            }
        }
        /// <summary>
        /// Backgound task functions
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            updateDataBase = new Timer(UpdateCash, null, TimeSpan.Zero,
                periodTime);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            updateDataBase?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }



    }
}
