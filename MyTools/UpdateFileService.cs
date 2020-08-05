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
    public class UpdateFileService: IHostedService, IUpdateFile
    {
        public static string IsoPath = @"Data/Iso.json";
        public static string RecentHistoryPath = @"Data/RecentHistory.json";
        public static string TestPath = @"Data/test.json";
        private string day { get;  set; }      
        private static Timer updateDataBase;
        private TimeSpan periodTime;


        public void UpdateCash(object state)
        {

            this.periodTime = CheckIsItWeekend();
            //sprawdz jak porownać konkretną godzine
           
            if (DateTime.UtcNow.Hour< 8)
            {
                int x = DateTime.UtcNow.Hour;
                for (int i = x; i < 24; i++)
                {
                    if (x == 8)
                    {
                        break;
                    }
                }

            }
            
            //string path = @"Data/RecentHistory.json";
            //path = Path.GetFullPath(path);
            //string fileContent = File.ReadAllText(path);
            // if (fileContent==String.Empty)
            // {
            //      File.WriteAllText(path, "XD");
            // }
            // else
            // {
            //      fileContent = File.ReadAllText(path);
            //      fileContent += "\nXD";
            //      File.WriteAllText(path, fileContent);
            // }

        }

        private TimeSpan CheckIsItWeekend()
        {
            if(DateTime.Today.DayOfWeek.Equals("Sunday") )
            {
                this.day = "Sunday";
                
            }
            else if (DateTime.Today.DayOfWeek.Equals("Saturday"))
            {
                this.day = "Saturday";
                return TimeSpan.FromHours(48);
            }
            return TimeSpan.FromHours(24);
        }

        //public void SaveToFile(string path,string text)
        //{
        //    path = Path.GetFullPath(path);
        //    string fileContent = File.ReadAllText(path);
        //    if (fileContent == String.Empty)
        //    {
        //        File.WriteAllText(path, text);
        //    }
        //    else
        //    {
        //        fileContent = File.ReadAllText(path);
        //        fileContent += "\n"+text;
        //        File.WriteAllText(path, fileContent);
        //    }
        //}
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
                    fileContent += "\n"+text;
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
