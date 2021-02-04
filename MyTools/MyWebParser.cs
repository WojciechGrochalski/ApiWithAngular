using angularapi.Models;
using AngularApi.DataBase;
using AngularApi.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AngularApi.MyTools
{
    public class MyWebParser : IWebParser
    {
        readonly WebClient webClient = new WebClient();
        List<CurrencyDBModel> listOfCurrency = new List<CurrencyDBModel>();
        CurrencyDBModel _currencyModel = new CurrencyDBModel();

        public CurrencyDBModel DownloadActualCurrency(string iso)
        {

            string url = "http://api.nbp.pl/api/exchangerates/rates/c/" + iso + "/?today/?format=json";
            string reply = webClient.DownloadString(url);
            dynamic jObject = JObject.Parse(reply);
            DateTime date = DateTime.Now;
            string name = jObject.currency;
            string code = jObject.code;
            float askPrice = jObject.rates[0].ask;
            float bidPrice = jObject.rates[0].bid;
            CurrencyDBModel _cashModel = new CurrencyDBModel(name, code, bidPrice, askPrice, date);

            return _cashModel;

        }

        public void SendCurrencyToDataBase(List<CurrencyDBModel> _listOfValue, CashDBContext _context)
        {

            if (!CheckDatabase(_context))
            {
                foreach (CurrencyDBModel cash in _listOfValue)
                {
                    _context.cashDBModels.Add(cash);

                }

                _context.SaveChanges();

            }

        }

        // Verify IF database was already updated today
        bool CheckDatabase(CashDBContext _context)
        {

            var query = _context.cashDBModels
                        .Where(s => s.Data.Date == DateTime.Today.Date).FirstOrDefault();

            if (query == null)
            {
                return false;
            }

            return true;

        }

        public void SaveToFile(string pathToFile, string text, bool appendText = false)
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


        public string[] GetIsoFromFile(string[] isoArray)
        {
            string path = @"Data/Iso.json";
            path = Path.GetFullPath(path);
            string fileData = File.ReadAllText(path);
            return _ = JsonConvert.DeserializeObject<string[]>(fileData);

        }
    }
}
