using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentitySample.Models
{
    public class GenerateTokenViewModel
    {
        public bool AlreadyGenerated { get; set; }
        public string Token { get; set; }
    }
}
