using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentitySample.Models.Database
{
    /// <summary>
    /// Rozszerzenie domyślnej klasy IdentityUser o dwie dodatkowe kolumny
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public bool TokenGenerated { get; set; }
        public DateTime? GenerationTimestamp { get; set; }
    }
}
