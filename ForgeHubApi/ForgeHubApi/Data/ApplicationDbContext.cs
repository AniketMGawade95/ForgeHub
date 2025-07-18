using ForgeHubApi.Models;
using ForgeHubProj.Models;
using Microsoft.EntityFrameworkCore;

namespace ForgeHubApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<RFQ> RFQs { get; set; }
        public DbSet<RFQQuotation> RFQQuotations { get; set; }
        public DbSet<FinalizedQuotation> FinalizedQuotations { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users (Buyer) → RFQs
            modelBuilder.Entity<RFQ>()
                .HasOne(r => r.Buyer)
                .WithMany(u => u.RFQs)
                .HasForeignKey(r => r.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Users (Vendor) → RFQQuotations
            modelBuilder.Entity<RFQQuotation>()
                .HasOne(q => q.Vendor)
                .WithMany(u => u.RFQQuotations)
                .HasForeignKey(q => q.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            // RFQ → RFQQuotations
            modelBuilder.Entity<RFQQuotation>()
                .HasOne(q => q.RFQ)
                .WithMany(r => r.RFQQuotations)
                .HasForeignKey(q => q.RFQId)
                .OnDelete(DeleteBehavior.Restrict);

            // RFQQuotation → FinalizedQuotation (1:1)
            modelBuilder.Entity<FinalizedQuotation>()
                .HasOne(f => f.RFQQuotation)
                .WithOne(q => q.FinalizedQuotation)
                .HasForeignKey<FinalizedQuotation>(f => f.QuotationId)
                .OnDelete(DeleteBehavior.Restrict);

            // RFQ → FinalizedQuotation (1:1)
            modelBuilder.Entity<FinalizedQuotation>()
                .HasOne(f => f.RFQ)
                .WithOne(r => r.FinalizedQuotation)
                .HasForeignKey<FinalizedQuotation>(f => f.RFQId)
                .OnDelete(DeleteBehavior.Restrict);
        }




    }
}
