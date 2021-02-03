using AngularApi.DataBase;
using AngularApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angularapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CashDBContext _context;


        public UserController(CashDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserDBModel user)
        {
            _context.userDBModels.Add(user);
            _context.SaveChanges();
            await Task.CompletedTask;
            return Ok();
        }
        [HttpPost("sub")]
        public async Task<IActionResult> AddSubscribtion(SubscriptionDBModel subb)
        {
            _context.subscriptionDBModels.Add(subb);
            _context.SaveChanges();
            await Task.CompletedTask;
            return Ok();
        }
    }
}
