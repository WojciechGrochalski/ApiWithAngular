using AngularApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi.Repository
{
   public interface IUserService
    {
        UserDBModel Authenticate(string username, string password);

        UserDBModel Create(UserDBModel user);
        //void Update(User user, string password = null);
        //void Delete(int id);
    }
}
