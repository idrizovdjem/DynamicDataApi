using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DynamicData.Data.Models;

namespace DynamicData.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<AccessToken> AccessTokens { get; set; }

        public DbSet<TableRecord> TableRecords { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
