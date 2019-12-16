using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentitySample.Models
{
    public class UserTokenInfo
    {
        public string UserId { get; set; }
        public bool TokenGenerated { get; set; }
    }
}
