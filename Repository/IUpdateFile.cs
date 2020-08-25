﻿using AngularApi.DataBase;
using AngularApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi.Repository
{
   public interface IUpdateFile
    {
        public CashDBModel DownloadActual(string iso);

        public void SendCurrencyToDataBase(List<CashDBModel> _listOfValue, CashDBContext _context);

        public void SaveToFile(string pathToFile, string text, bool appendText);

        public string[] GetIsoFromFile(string[] isoArray);
    }
}
