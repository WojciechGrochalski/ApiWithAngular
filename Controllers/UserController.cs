using AngulaAapi.Models;
using AngularApi.DataBase;
using AngularApi.Models;
using AngularApi.Repository;
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
        private IUserService _userService;

        public UserController(CashDBContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost]
        public IActionResult AddUser(UserDBModel user)
        {
            try
            {
                _userService.Create(user);
                return Ok();
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthModel model)
        {
            var user = _userService.Authenticate(model.Name, model.Pass);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(new { 
            ID=user.ID,
            Name=user.Name,
            Email=user.Email,
            Subscribtion=user.Subscriptions
            });
        }
        [HttpPost("sub")]
        public async Task<IActionResult> AddSubscribtion(Remainder subb)
        {
            _context.Remainders.Add(subb);
            _context.SaveChanges();
            await Task.CompletedTask;
            return Ok();
        }
    }
}
