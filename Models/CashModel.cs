using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi
{
    public class CashModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string BidPrice { get;  set; }
        public string AskPrice { get; set; }
        public string Data { get; set; }

        public static List<CashModel> cashModelsList = new List<CashModel>();

        public void GetData()
        {
            string path = @"Data/ValueOfCurrencyToday.json";
            path = Path.GetFullPath(path);

            string fileData = File.ReadAllText(path);
            cashModelsList =JsonConvert.DeserializeObject<List<CashModel>>(fileData);
        }
    }
}

