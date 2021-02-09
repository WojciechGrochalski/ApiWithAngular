using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angularapi.Models
{
    public class Remainder
    {
        public int ID { get; set; }
        public bool? MoreThan { get; set; }
        public string Currency { get; set; }
        public float? BidPrice { get; set; }
        public float? AskPrice { get; set; }
        public int UserID { get; set; }

        public  UserDBModel User { get; set; }
    }
}
