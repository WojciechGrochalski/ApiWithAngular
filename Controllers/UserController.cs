﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using angularapi.Models;
using AngularApi.DataBase;
using AngularApi.Repository;
using Microsoft.AspNetCore.Mvc;
using angularapi.MyTools;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace angularapi.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public IActionResult Login(AuthModel model)
        {
            (var user, string refreshToken) = _userService.AuthenticateLogin(model.Name, model.Pass);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(new
            {
                ID = user.ID,
                Name = user.Name,
                AccessToken = TokenManager.GenerateAccessToken(user.Name),
                RefreshToken = refreshToken
            });
        }
        [Authorize(AuthenticationSchemes = "refresh")]
        [HttpPut("refreshToken", Name = "refresh")]
        public IActionResult RefreshToken()
        {
            Claim refreshtoken = User.Claims.FirstOrDefault(x => x.Type == "refresh");
            Claim username = User.Claims.FirstOrDefault(x => x.Type == "user");
            try
            {
                Tokens tokens = _userService.Refresh(username, refreshtoken);
                return Ok(new
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken
                }) ;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
            bool result = _userService.VerifyEmail(verifyEmail.Token);
            if (result)
            {
                return Ok(new { message = "Verification successful, you can now login" });
            }
            return BadRequest(new { message = "Invalid access token" });
        }

        [HttpPost("baseUrl")]
        [AllowAnonymous]
        public IActionResult GetBaseUrl([FromBody] BaseUrlModel baseUrl)
        {
            BaseUrl = baseUrl.BaseUrl;
            return Ok();
        }

        [HttpGet("sub/{userID}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [HttpGet("remove/sub/{userID}")]
        public IActionResult RemoveSubscriptions(int userID)
        {
            var user = _context.userDBModels.FirstOrDefault(s => s.ID == userID);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            user.Subscriptions = false;
            _context.Entry(user).CurrentValues.SetValues(user);
            _context.SaveChanges();
            return Ok(new { message = "Usnieto cię z subskrybcji" });
        }

        [HttpPost("addAlert")]
        public IActionResult AddAlert([FromBody] Remainder remainder)
        {
            try
            {
                var user = _context.userDBModels.FirstOrDefault(s => s.ID == remainder.UserID);
                if (user != null)
                {
                    _context.Remainders.Add(remainder);
                    _context.SaveChanges();
                    return Ok(new { message = "Pomyślnie ustawiono alert" });
                }
                return BadRequest(new { message = "Nie udało się ustawić alertu, spróbuj ponownie" });
            }
            catch (ApplicationException e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new { message = "Nie udało się ustawić alertu, spróbuj ponownie" });
            }

        }
    }
}
