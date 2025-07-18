using ForgeHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ForgeHubApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}
