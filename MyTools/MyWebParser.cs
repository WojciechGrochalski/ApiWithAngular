using AngularApi.DataBase;
using AngularApi.Models;
using AngularApi.Repository;
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
    public class MyWebParser:IUpdateFile
    {
         readonly WebClient webClient = new WebClient();
         List<CashDBModel> _listCashDBContexts = new List<CashDBModel>();
         CashDBModel _cashModel = new CashDBModel();
         private CashDBContext _context;

         string[] isoArray;
         string url;
         string reply;


        
        public MyWebParser( CashDBContext context)
        {
            _context = context;
        }
        public  List<CashDBModel> DownloadActual()
        {
            GetIsoFromFile(ref isoArray);
            foreach (var iso in isoArray)
            {
                url = "http://api.nbp.pl/api/exchangerates/rates/c/" + iso + "/?today/?format=json";
                reply = webClient.DownloadString(url);
                dynamic jObject = JObject.Parse(reply);
                _cashModel.Name = jObject.currency;
                _cashModel.Code = jObject.code;
                _cashModel.AskPrice = jObject.rates[0].ask;
                _cashModel.BidPrice = jObject.rates[0].bid;
                _cashModel.Data = DateTime.Now.Date;
                _listCashDBContexts.Add(_cashModel);
            }

            return _listCashDBContexts;
        }

        public void SendCurrencyToDataBase(List<CashDBModel> _listOfValue)
        {
            foreach (CashDBModel cash in _listOfValue)
            {
                _context.cashDBModels.Add(cash);
                _context.SaveChanges();
                string LogsMessage = cash.ToString() + "Wpisano: " + DateTime.UtcNow.ToString();
                SaveToFile(UpdateFileService.LogsPath, LogsMessage, true);
               
            }
           
        }

        public  void SaveToFile(string pathToFile, string text,  bool  appendText=false)
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
