
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace angularapi.Models
{
    public class UserDBModel 
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string VeryficationToken { get; set; }
        public bool IsVerify { get; set; }
        public DateTime Created { get; set; }
        public bool? Subscriptions { get; set; }
        public  List<Remainder> Remainder { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
