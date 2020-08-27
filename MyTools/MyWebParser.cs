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

namespace AngularApi.MyTools
{
    public class MyWebParser : IUpdateFile
    {
        readonly WebClient webClient = new WebClient();
        List<CashDBModel> listOfCash = new List<CashDBModel>();
        CashDBModel _cashModel = new CashDBModel();
      
        string url;
        string reply;

        public CashDBModel DownloadActual(string iso)
        {

            url = "http://api.nbp.pl/api/exchangerates/rates/c/" + iso + "/?today/?format=json";
            reply = webClient.DownloadString(url);
            dynamic jObject = JObject.Parse(reply);
            DateTime date = DateTime.Now;
            string name = jObject.currency;
            string code = jObject.code;
            string askPrice = jObject.rates[0].ask;
            string bidPrice = jObject.rates[0].bid;
            CashDBModel _cashModel= new CashDBModel(name, code, bidPrice, askPrice, date);

            return _cashModel;

        }

        public void SendCurrencyToDataBase(List<CashDBModel> _listOfValue, CashDBContext _context)
        {

            if (!CheckDatabase(_context))
            {
                foreach (CashDBModel cash in _listOfValue)
                {
                     _context.cashDBModels.Add(cash);

                    string LogsMessage = cash.Code.ToString() + "  Wpisano: " + DateTime.Now.ToString();
                    SaveToFile(UpdateFileService.LogsPath, LogsMessage, true);

                }

              _context.SaveChanges();

            }

        }

        bool CheckDatabase(CashDBContext _context)
        {

            var query = _context.cashDBModels
                        .Where(s => s.Data.Date == DateTime.Today.Date).FirstOrDefault<CashDBModel>();
                        

            //today database dont be updated
            if (query ==null)
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

        // dodać referencje
        public string[] GetIsoFromFile(string[] isoArray)
        {
            string path = @"Data/Iso.json";
            path = Path.GetFullPath(path);
            string fileData = File.ReadAllText(path);
            return _ = JsonConvert.DeserializeObject<string[]>(fileData);

        }
    }
}
