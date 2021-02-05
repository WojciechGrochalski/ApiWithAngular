using angularapi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi.DataBase
{
    public class CashDBContext : DbContext
    {
      
        public CashDBContext(DbContextOptions<CashDBContext> options) : base (options)
        {

        }
        public DbSet<CurrencyDBModel> cashDBModels { get; set; }
        public DbSet<UserDBModel> userDBModels { get; set; }
        public DbSet<Remainder> Remainders { get; set; }

    }
}
