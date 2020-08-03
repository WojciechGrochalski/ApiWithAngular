using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AngularApi.MyTools
{
    public class MyWebParser
    {
         readonly WebClient webClient = new WebClient();
         string[] isoArray;
         string url;
         string reply;


         List<CashModel> _listofCashCurrency = new List<CashModel>();
         CashModel _cashModel = new CashModel();
        public List<CashModel> DownloadActual()
        {
            GetIsoFromFile(ref isoArray);
            foreach (var iso in isoArray)
            {
                url= "http://api.nbp.pl/api/exchangerates/rates/c/" + iso + "/?today/?format=json";
                reply = webClient.DownloadString(url);
                dynamic jObject = JObject.Parse(reply);
                _cashModel.Name = jObject.currency;
                _cashModel.Code = jObject.code;
                _cashModel.AskPrice = jObject.rates[0].ask;
                _cashModel.BidPrice = jObject.rates[0].bid;
                _cashModel.Data= DateTime.Now.ToString("dd.MM.yyyy").ToString();
                _listofCashCurrency.Add(_cashModel);
            }

            return _listofCashCurrency;
        }

        // dodać referencje
        private void GetIsoFromFile(ref string[] isoArray)
        {
            string path = @"Data/Iso.json";
            path = Path.GetFullPath(path);
            string fileData = File.ReadAllText(path);
            isoArray = JsonConvert.DeserializeObject<string[]>(fileData);
        }
    }
}
