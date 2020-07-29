using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;





namespace AngularApi.Controllers
{
    [Route("api/[controller]")]
    public class CashController : Controller
    {
        private readonly ILogger<CashController> _logger;
        CashModel cashModel = new CashModel();

        public CashController(ILogger<CashController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public CashModel[] Get()
        {
            //return foreach item in CashModel
            //{
            //    Data = DateTime.Now.AddDays(2),
            //    AskPrice = "25",
            //    BidPrice="48",
            //    Code="USD"
            //}).ToArray();
            cashModel.GetData();
          
            return CashModel.cashModelsList.ToArray();
        }

       
    }
}
