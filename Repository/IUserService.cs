using angularapi.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi.Repository
{
   public interface IUserService
    {
        UserDBModel Authenticate(string username, string password);

        UserDBModel CreateAsync(UserDBModel user);
        void VerifyEmail(string token);


        //void Update(User user, string password = null);
        //void Delete(int id);
    }
}
