using angularapi.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AngularApi.Repository
{
   public interface IUserService
    {
        (UserDBModel, string) AuthenticateLogin(string username, string password);

        UserDBModel CreateAsync(UserDBModel user);
        bool VerifyEmail(string token);

        bool VerifyPasswordToken(string token);
        Tokens Refresh(Claim username, Claim refreshtoken);
        //void Update(User user, string password = null);
        //void Delete(int id);
    }
}
