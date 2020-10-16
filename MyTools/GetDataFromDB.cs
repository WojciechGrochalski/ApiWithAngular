using AngularApi;
using AngularApi.DataBase;
using AngularApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angularapi.MyTools
{
    public class GetDataFromDB
    {
        List<CashModel> query = new List<CashModel>();
   

        public List<CashDBModel> GetTodayAllCurrency(CashDBContext _context)
        {
            return _context.cashDBModels.OrderByDescending(s => s.ID).Take(13).ToList();
        }

        public CashDBModel GetLastOne(string iso, CashDBContext _context)
        {
            return _context.cashDBModels.OrderByDescending(s => s.ID).Where(s => s.Code == iso).FirstOrDefault();
        }
        public List<CashDBModel> GetLastCountNumberOfCurrency(string iso, int count, CashDBContext _context)
        {
            return _context.cashDBModels.Where(s => s.Code == iso).OrderByDescending(s => s.ID).Take(count).ToList();
        }

    }
}
