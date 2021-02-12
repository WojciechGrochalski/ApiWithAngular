
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angularapi.Models
{
    public class RefreshToken
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public int UserID { get; set; }

    }
}
