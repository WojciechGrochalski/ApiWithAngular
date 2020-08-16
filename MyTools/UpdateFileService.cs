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


namespace AngularApi.MyTools
{
    public class UpdateFileService : IHostedService
    {
        public static string IsoPath = @"Data/Iso.json";
        public static string RecentHistoryPath = @"Data/RecentHistory.json";
        public static string TestPath = @"Data/test.json";
        public static string LogsPath = @"Data/Logs.txt";
        private string day { get; set; }
        private static Timer updateDataBase;
        private TimeSpan periodTime;
       

        MyWebParser parser = new MyWebParser();



        List<CashDBModel> listOfCash = new List<CashDBModel>();


        public void UpdateCash(object state)
        {


            periodTime = SetTimer();

            if (DateTime.Today.DayOfWeek.ToString() != "Sunday" &&
                   DateTime.Today.DayOfWeek.ToString() != "Saturnday")
            {


                listOfCash = parser.DownloadActual();
                parser.SendCurrencyToDataBase(listOfCash);

            }

        }



        private TimeSpan SetTimer()
        {

            return SetMinuts();
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
