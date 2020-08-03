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
        CashModel _cashModel = new CashModel();

        [HttpGet]
        public List<CashModel> Get()
        {
            _cashModel.GetData();

            return CashModel.cashModelsList;//.ToArray();
        }

       
    }
}
