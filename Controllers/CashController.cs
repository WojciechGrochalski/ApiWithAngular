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

namespace AngularApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CashController : ControllerBase
    {
       
        private readonly CashDBContext _context;

        public CashController(CashDBContext context)
        {
            _context = context;
        }
     
      

        [HttpGet]
        public async Task<List<CashModel>> GetLastCurrency()
        {

            var query = _context.cashDBModels.OrderByDescending(s => s.ID).Take(13).ToList();
            await Task.CompletedTask;
            List<CashModel> list = new List<CashModel>();
            CashModel cashModel;
            foreach (CashDBModel item in query)
            {
                list.Add(cashModel = new CashModel(item));
            }
            list.Reverse();
            return list;
        }


        [HttpGet("{iso}")]
        public async Task<CashDBModel> GetLastOneCurrency()
        {

            var query = _context.cashDBModels.OrderByDescending(s => s.ID).FirstOrDefault();
            await Task.CompletedTask;
            return query;

        }

        [HttpGet("{iso}/{count}")]
        public async Task<List<CashDBModel>> GetLastManyCurrency(string iso, int count)
        {

            var query = _context.cashDBModels.Where(s =>s.Code==iso).OrderByDescending(s => s.ID).Take(count).ToList();
            await Task.CompletedTask;
            return query;

        }

        [HttpPost("{msg}")]
        public  /*Task<ActionResult<string>>*/ string Post([FromBody] string message)
        {
            //user = new UserDBModel();
            //user.Name = message;
            //user.Pass = message;
            //user.Email = message;
            //_context.userDBModels.Add(user);
            //_context.SaveChangesAsync();
            UpdateFileService.SaveToFile(UpdateFileService.TestPath, message, false);

            return message;
            //return CreatedAtAction(nameof(message), new { id = message }, message);
        }


    }
}
