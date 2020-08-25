using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularApi.Models
{
    public class CashDBModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string BidPrice { get; set; }
        public string AskPrice { get; set; }
        public DateTime Data { get; set; }

        public CashDBModel()
        {

        }
        public CashDBModel(string name, string code, string bidPrice, string askPrice, DateTime data)
        {
            Name = name;
            Code = code;
            BidPrice = bidPrice;
            AskPrice = askPrice;
            Data = data;
        }
    }
}
