using AngularApi.DataBase;
using AngularApi.Models;
using AngularApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angularapi.MyTools
{
    public class UserService : IUserService
    {
        private readonly CashDBContext _context;


        public UserService(CashDBContext context)
        {
            _context = context;
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
            if (!SecurePasswordHasher.Verify(password, user.Pass)){
                return null;
            }

            // authentication successful
            //user.Pass = null;
            return user;
        }
        public UserDBModel Create(UserDBModel user)
        {
            // validation
            if (string.IsNullOrWhiteSpace(user.Pass))
            {
                throw new ApplicationException("Password is required");
            }

            if (_context.userDBModels.Any(x => x.Name == user.Name))
            {
                throw new ApplicationException("Username \"" + user.Name + "\" is already taken");
            }

            string PasswordHash = SecurePasswordHasher.Hash(user.Pass);

            user.Pass = PasswordHash;

            _context.userDBModels.Add(user);
            _context.SaveChanges();

            return user;
        }

       
    }
}
