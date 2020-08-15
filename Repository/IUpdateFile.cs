using AngularApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi.Repository
{
   public interface IUpdateFile
    {
        public List<CashDBModel> DownloadActual();

        public void SendCurrencyToDataBase(List<CashDBModel> _listOfValue);

        public void SaveToFile(string pathToFile, string text, bool appendText);




    }
}
