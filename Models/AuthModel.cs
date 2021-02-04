using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AngulaAapi.Models
{
    public class AuthModel
    {
          
        [Required]
        public string Name { get; set; }

        [Required]
        public string Pass { get; set; }
    }
}

