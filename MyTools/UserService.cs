using angularapi.Controllers;
using angularapi.Models;
using angularapi.Repository;
using AngularApi.DataBase;
using AngularApi.Repository;
using Hangfire;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace angularapi.MyTools
{
    public class UserService : IUserService
    {
        private readonly CashDBContext _context;
        private readonly ILogger<BackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly AppSettings _appSettings;


    

        string subject = "Sign-up Verification API - Verify Email";
        public UserService(CashDBContext context,
            IOptions<AppSettings> appSettings,
            ILogger<BackgroundService> logger,
            IServiceScopeFactory scopeFactory  )
        {
            _context = context;
            _appSettings = appSettings.Value;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }


        public UserDBModel Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _context.userDBModels.SingleOrDefault(x => x.Name == username);

            // check if username exists
            if (user == null)
            {
                return null;
            }

            // check if password is correct
            if (!SecurePasswordHasher.Verify(password, user.Pass))
            {
                return null;
            }

            // authentication successful
            //user.Pass = null;
            return user;
        }
        public UserDBModel CreateAsync(UserDBModel user)
        {
            // validation
            if (string.IsNullOrWhiteSpace(user.Pass))
            {
                throw new ApplicationException("Password is required");
            }

            if (_context.userDBModels.Any(x => x.Name == user.Name))
            {
                throw new ApplicationException("Username " + user.Name + " is already taken");
            }
            if (_context.userDBModels.Any(x => x.Email == user.Email))
            {
                throw new ApplicationException("Email " + user.Email + " is already taken");
            }

            string PasswordHash = SecurePasswordHasher.Hash(user.Pass);

            user.Pass = PasswordHash;
            user.Created = DateTime.Now;
            user.IsVerify = false;
            user.VeryficationToken = RandomTokenString();
            _context.userDBModels.Add(user);
            _context.SaveChanges();

            string message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p> <a href=""{UserController.BaseUrl}user-profile?token={TokenManager.GenerateAccessToken(user.VeryficationToken)}""> link <a/> </p>";
           
            SendVeryficationToken("cash-service@email.com", user.Email, subject, message);
            BackgroundTask task = new BackgroundTask(_logger, _scopeFactory);
            Task.Factory.StartNew(() => task.RemoveUnverifiedUserAsync(user.VeryficationToken));
           
            return user;
        }
        public bool VerifyEmail(string token)
        {
            try
            {
                string verifyToken = TokenManager.ValidateJwtToken(token);
                if (verifyToken == null)
                {
                    return false;
                }
                var account = _context.userDBModels.SingleOrDefault(x => x.VeryficationToken == verifyToken);

                if (account == null)
                {
                    return false;
                }
                account.IsVerify = true;
                account.VeryficationToken = null;

                _context.userDBModels.Update(account);
                _context.SaveChanges();
                return true;
            }
            catch (ApplicationException e)
            {
                throw new ApplicationException(e.Message);
            }
           
        }


        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        public void SendVeryficationToken(string from, string to, string subject, string html)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
