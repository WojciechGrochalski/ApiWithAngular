using AngularApi.DataBase;
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
using TimeZoneConverter;
using angularapi.Models;

namespace AngularApi.MyTools
{

    public class UpdateFileService : IHostedService
    {
        public static string IsoPath = @"Data/Iso.json";
        public static string RecentHistoryPath = @"Data/RecentHistory.json";
        public static string TestPath = @"Data/test.json";
        public static string LogsPath = @"Data/Logs.txt";

        private static Timer updateDataBase;

        private TimeSpan periodTime = TimeSpan.FromMinutes(5);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger _logger;



        public UpdateFileService(ILogger<UpdateFileService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        List<CurrencyDBModel> listOfCash = new List<CurrencyDBModel>();
        string[] isoArray;

        public void UpdateCash(object state)
        {
            periodTime = SetMinuts(DateTime.Now);
            using (var scope = _scopeFactory.CreateScope())
            {
                var _update = scope.ServiceProvider.GetRequiredService<IWebParser>();
                var _mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
                var _context = scope.ServiceProvider.GetRequiredService<CashDBContext>();

                if (DateTime.Today.DayOfWeek.ToString() != "Saturday" &&
                       DateTime.Today.DayOfWeek.ToString() != "Sunday")
                {
                    if (ChceckItIsAvailableApi())
                    {
                        isoArray = _update.GetIsoFromFile(isoArray);
                        foreach (string iso in isoArray)
                        {
                            listOfCash.Add(_update.DownloadActualCurrency(iso));
                        }

                        _update.SendCurrencyToDataBase(listOfCash, _context);
                        SendTodayCurrencyToSubscribers(_context, _mailService);
                    }
                }
            }
        }

        private string MakeMessage()
        {
            string table = $@"";
            foreach (CurrencyDBModel item in listOfCash)
            {
                table += $@" < td >{item.Data}</ td >
                             < td >{item.Name}</ td >
                             < td >{item.AskPrice } PLN </ td >
                             < td >{item.BidPrice} PLN </ td >";
            }
            string message = $@"<p>Dzisiejsze kursy walut:</p><br><br><br><br>
                              <thead> <tr>
                             <th>Data</th>
                            <th>Waluta</th>
                            <th>Cena kupna (Za tyle kupisz)</th>
                            <th>Cena sprzedaży (Za tyle sprzedasz)</th>
                             <th></th>";

           return message += table + $@"</ tr ></ tbody > ";
        }
        private void SendTodayCurrencyToSubscribers(CashDBContext _context, IMailService _mailService)
        {
            string message = MakeMessage();
            var users = _context.userDBModels.Where(s => s.IsVerify).ToList();
            if (users != null)
            {
                foreach (UserDBModel item in users)
                {
                    _mailService.SendMail(item.Email, "Kurs walut", message);
                }
            }
        }
        private bool ChceckItIsAvailableApi()
        {
            TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo("Central Europe Standard Time");
            DateTime utcNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
            if (utcNow.Hour > 8 && utcNow.Hour < 22)
            {
                return true;
            }
            if (utcNow.Hour == 8 && utcNow.Minute > 16)
            {
                return true;
            }
            return false;
        }

        public TimeSpan SetMinuts(DateTime dateTime)
        {
            if (dateTime.Hour < 8)
            {
                int hour = 0;
                int acctualhour = dateTime.Hour;
                for (int i = acctualhour; i < 24; i++)
                {
                    hour++;
                    if (acctualhour == 8)
                    {
                        break;
                    }
                }
                return TimeSpan.FromHours(hour + 24) + TimeSpan.FromMinutes(16);
            }
            if (dateTime.Hour > 8)
            {
                int acctualhour = dateTime.Hour;
                int hour = 0;
                for (int i = acctualhour; i < 24; i++)
                {
                    hour++;
                }
                return TimeSpan.FromHours(hour + 8) + TimeSpan.FromMinutes(16);
            }
            return TimeSpan.FromHours(24) + TimeSpan.FromMinutes(16);
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
