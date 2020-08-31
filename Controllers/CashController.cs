﻿using System;
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
        CashModel _cashModel = new CashModel();
        private readonly CashDBContext _context;

        public CashController(CashDBContext context)
        {
            _context = context;
        }
     
      

        [HttpGet]
        public List<CashModel> Get()
        {
            _cashModel.GetData();

            return CashModel.cashModelsList;//.ToArray();
        }


        [HttpGet("{iso}")]
        public async Task<CashDBModel> GetLastCurrency()
        {

            var query = _context.cashDBModels.OrderByDescending(s => s.ID).FirstOrDefault();
            await Task.CompletedTask;
            return query;

        }

        [HttpGet("{iso}/{count}")]
        public async Task<List<CashDBModel>> GetLastManyCurrency(int count)
        {

            var query = _context.cashDBModels.Where(s =>s.Code=="USD").OrderByDescending(s => s.ID).Take(count).ToList();
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
