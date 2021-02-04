using angularapi.Models;
using angularapi.Repository;
using AngularApi.DataBase;
using AngularApi.Repository;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppSettings _appSettings;
        public UserService(CashDBContext context,
            UserManager<ApplicationUser> userManager,
            IOptions<AppSettings> appSettings )
        {
            _context = context;
            _userManager = userManager;
            _appSettings = appSettings.Value;
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

            string PasswordHash = SecurePasswordHasher.Hash(user.Pass);

            user.Pass = PasswordHash;
            user.Created = DateTime.UtcNow;
            user.IsVerify = false;
            user.VeryficationToken = randomTokenString();
            _context.userDBModels.Add(user);
            _context.SaveChanges();

            string message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p>{user.VeryficationToken} </p>";
            string subject = "Sign-up Verification API - Verify Email";
            SendVeryficationToken("cash-service@email.com", user.Email, subject, message);
            return user;
        }
        public void VerifyEmail(string token)
        {
            var account = _context.userDBModels.SingleOrDefault(x => x.VeryficationToken == token);

            if (account == null)
            {
                throw new ApplicationException("Verification failed");
            }

            account.IsVerify = true;
            account.VeryficationToken = null;

            _context.userDBModels.Update(account);
            _context.SaveChanges();
        }


        private string randomTokenString()
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
