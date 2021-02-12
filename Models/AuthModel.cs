using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace angularapi.Models
{
    public class AuthModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Pass { get; set; }
    }
}

