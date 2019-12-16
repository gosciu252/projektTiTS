using System;
using System.Collections.Generic;
using System.Text;
using AspNetIdentitySample.Models.Database;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetIdentitySample.Data
{
    /// <summary>
    /// Kontekst bazodanowy EntityFramework wykorzystujący rozszerzoną klasę ApplicationUser
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Token> Tokens { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
    }
}
