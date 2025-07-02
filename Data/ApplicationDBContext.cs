using System.Collections.Generic;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    // ApplicationDBContext adalah turunan dari DbContext
    public class ApplicationDBContext : DbContext
    {
        // Konstruktor menerima opsi koneksi ke DB
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        // DbSet akan otomatis membuat tabel di DB
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        // Konfigurasi hubungan antar entitas dan seeding data
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Konfigurasi composite key untuk Portfolio
            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            builder.Entity<Portfolio>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>()
                .HasOne(p => p.Stock)
                .WithMany(s => s.Portfolios)
                .HasForeignKey(p => p.StockId);

            // Seed role data dengan Id yang statis
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // (Opsional) Tentukan tipe kolom decimal agar warning hilang
            builder.Entity<Stock>()
                .Property(s => s.Purchase)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Stock>()
                .Property(s => s.LastDiv)
                .HasColumnType("decimal(18,2)");
        }
    }
}
