using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using AngularApi.MyTools;
using AngularApi.Repository;
using AngularApi.DataBase;
using Microsoft.EntityFrameworkCore;
using AngularApi.Models;
using Microsoft.AspNetCore.Cors;
using System.Globalization;
using angularapi.MyTools;

namespace AngularApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowOrigin")]
    public class CashController : ControllerBase
    {

        private readonly CashDBContext _context;
        GetDataFromDB get = new GetDataFromDB();

        public CashController(CashDBContext context)
        {
            _context = context;
        }



        [HttpGet]
        [EnableCors("AllowOrigin")]
        public async Task<List<CashModel>> GetLastCurrency()
        {
            List<CashModel> list = new List<CashModel>();
            CashModel cashModel;
            List<CashDBModel> query;
            query = get.GetTodayAllCurrency(_context);

            foreach (CashDBModel item in query)
            {
                list.Add(cashModel = new CashModel(item));
            }

            list.Reverse();
            await Task.CompletedTask;
            return list;
        }


        [HttpGet("{iso}")]
        [EnableCors("AllowOrigin")]
        public async Task<CashModel> GetLastOneCurrency(string iso)
        {
            iso.ToUpper();
            CashDBModel query = get.GetLastOne(iso, _context);
            CashModel result = new CashModel(query);
            await Task.CompletedTask;
            return result;

        }

        [HttpGet("{iso}/{count}")]
        [EnableCors("AllowOrigin")]
        public async Task<List<CashModel>> GetLastManyCurrency(string iso, int count)
        {
            List<CashModel> list = new List<CashModel>();
            CashModel cashModel;
            List<CashDBModel> query;

            query = get.GetLastCountNumberOfCurrency(iso, count, _context);

            foreach (CashDBModel item in query)
            {
                list.Add(cashModel = new CashModel(item));
            }
            await Task.CompletedTask;
            return list;

        }
        /// <summary>
        ///     return date of cash
        /// </summary>
        /// <param name="iso"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet("{iso}/{count}/DataChart")]
        [EnableCors("AllowOrigin")]
        public async Task<string[]> GetDataChart(string iso, int count)
        {
            List<CashDBModel> query;
            query = get.GetLastCountNumberOfCurrency(iso, count, _context);

            string[] chartData = new string[query.Count];
            int i = 0;

            foreach (CashDBModel item in query)
            {
                chartData[i] = item.Data.ToShortDateString();
                i++;
            }
            Array.Reverse(chartData);
            await Task.CompletedTask;
            return chartData;
        }
        /// <summary>
        /// return Bid or Ask price
        /// </summary>
        /// <param name="iso"></param>
        /// <param name="count"></param>
        /// <param name="chartPrice"></param>
        /// <returns></returns>
        [HttpGet("{iso}/{count}/{chartPrice}")]
        [EnableCors("AllowOrigin")]
        public async Task<float[]> GetPriceChart(string iso, int count, string chartPrice)
        {
            List<CashDBModel> query;
            query = get.GetLastCountNumberOfCurrency(iso, count, _context);

            float[] chartData = new float[query.Count];
            int i = 0;
            if (chartPrice == "AskPrice")
                foreach (CashDBModel item in query)
                {
                    {
                        chartData[i] = item.AskPrice;
                        i++;
                    }
                }
            else if (chartPrice == "BidPrice")
            {
                foreach (CashDBModel item in query)
                {
                    chartData[i] = item.BidPrice;
                    i++;
                }
            }
            Array.Reverse(chartData);
            await Task.CompletedTask;
            return chartData;
        }





    }
}
