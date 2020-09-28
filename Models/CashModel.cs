using AngularApi.Models;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi
{
    public class CashModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public float BidPrice { get; set; }
        public float AskPrice { get; set; }
        public string Data { get; set; }



        public static List<CashModel> cashModelsList = new List<CashModel>();

        public CashModel()
        {

        }
        public CashModel(CashDBModel modelDB)
        {
            Name = modelDB.Name;
            Code = modelDB.Code;
            BidPrice = modelDB.BidPrice;
            AskPrice = modelDB.AskPrice;
            Data = modelDB.Data.ToString("f",
                  CultureInfo.CreateSpecificCulture("pl-PL"));
        }

        public void GetData()
        {
            string path = @"Data/ValueOfCurrencyToday.json";
            path = Path.GetFullPath(path);

            string fileData = File.ReadAllText(path);
            cashModelsList =JsonConvert.DeserializeObject<List<CashModel>>(fileData);
        }
    }
}

