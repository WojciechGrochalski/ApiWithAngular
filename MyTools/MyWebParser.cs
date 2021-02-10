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
        private IMailService _mailService;

        public MyWebParser(IMailService mailService)
        {
            _mailService = mailService;
        }
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
                SendTodayCurrencyToSubscribers(_context, _mailService, _listOfValue);
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
        private string MakeMessage(List<CurrencyDBModel> listOfCash)
        {
            string table = $@"";
            foreach (CurrencyDBModel item in listOfCash)
            {
                table += $@" <tr><td>{item.Data.ToShortDateString()}</td>
                             <td>{item.Name}</td >
                             <td>{item.AskPrice } PLN </td>
                             <td>{item.BidPrice} PLN </td></tr>";
            }
            string message = $@"<font face='Arial' size='6px'><p>Dzisiejsze kursy walut:</p></font><br>
                              <font face='Arial' size='5px'>
                              <table  border="" + 1 + "" cellpadding="" + 0 + "" cellspacing="" + 0 + "" width = "" + 500""><thead><tr>
                             <th>Data</th>
                             <th>Waluta</th>
                             <th>Cena kupna </th>
                             <th>Cena sprzedaży </th>
                             </tr></thead><tbody>";

            return message += table + $@"</tr></tbody></table></font>";
        }
        private void SendTodayCurrencyToSubscribers(CashDBContext _context, IMailService _mailService, List<CurrencyDBModel> listOfCash)
        {
            string message = MakeMessage(listOfCash);
            var users = _context.userDBModels.Where(s => s.IsVerify == true).ToList();
            if (users != null)
            {
                foreach (UserDBModel item in users)
                {
                    _mailService.SendMail(item.Email, "Kurs walut", message);
                }
            }
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
