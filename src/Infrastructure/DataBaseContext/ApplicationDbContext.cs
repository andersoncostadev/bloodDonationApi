using Domain.Entities.v1;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<DonationEntity>? Donations { get; set; }
        public DbSet<DonorEntity>? Donors { get; set; }
        public DbSet<AddressEntity>? Addresses { get; set; }
        public DbSet<StockBloodEntity>? StockBloods { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=localhost;Database=BloodDonationDb;User Id=sa;Password=Password123;");

            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonationEntity>()
                .HasOne(d => d.Donor)
                .WithMany(d => d.Donations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DonorEntity>()
                .HasOne(d => d.Address)
                .WithOne(a => a.Donor)
                .HasForeignKey<AddressEntity>(a => a.DonorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AddressEntity>()
                .HasKey(a => a.DonorId);
        }
    }
}
