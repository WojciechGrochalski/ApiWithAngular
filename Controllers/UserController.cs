using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using angularapi.Models;
using AngularApi.DataBase;
using AngularApi.Repository;
using Microsoft.AspNetCore.Mvc;
using angularapi.MyTools;
using Microsoft.Extensions.Logging;

namespace angularapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CashDBContext _context;
        private readonly ILogger<UserController> _logger;
        private IUserService _userService;
        public static string BaseUrl;

        public UserController(CashDBContext context, IUserService userService,
            ILogger<UserController> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Register(UserDBModel user)
        {
            try
            {
                _userService.CreateAsync(user);
                return Ok(new { message = "Registration successful, please check your email for verification instructions" });
            }
            catch (ApplicationException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("login")]
        public IActionResult Login( AuthModel model)
        {
            var user = _userService.Authenticate(model.Name, model.Pass);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(new
            {
                ID = user.ID,
                Name = user.Name,
                Email = user.Email,
                Subscribtion = user.Subscriptions
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

        //[HttpPost("ForgotPassword")]
        //public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
        //    if (user == null)
        //    {
        //        return BadRequest("Invalid Request");
        //    }
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var param = new Dictionary<string, string>
        //    {
        //         {"token", token },
        //         {"email", forgotPasswordDto.Email }
        //     };
        //    var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
        //    //var message = new Message(new string[] { "codemazetest@gmail.com" }, "Reset password token", callback, null);
        //    //await _emailSender.SendEmailAsync(message);
        //    return Ok(
        //       new
        //       {
        //           callback = callback
        //       }); ;
        //}
        [HttpPost("verify-email")]
        public IActionResult VerifyEmail([FromBody] VerifyEmailRequest verifyEmail)
        {
            bool result= _userService.VerifyEmail(verifyEmail.Token);
            if (result)
            {
                return Ok(new { message = "Verification successful, you can now login" });
            }
            return BadRequest(new { message = "Invalid access token" });
        }

        [HttpPost("baseUrl")]
        public IActionResult GetBaseUrl([FromBody] BaseUrlModel baseUrl)
        {
            BaseUrl = baseUrl.BaseUrl;   
            return Ok();
        }

        [HttpGet("sub/{userID}")]
        public IActionResult AddSubscriptions(int userID)
        {
            var user = _context.userDBModels.FirstOrDefault(s => s.ID == userID);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            user.Subscriptions = true;
            _context.Entry(user).CurrentValues.SetValues(user);
            _context.SaveChanges();
            return Ok(new { message = "Zostałeś dodany do subskrybcji" });
        }
    }
}
