using Microsoft.EntityFrameworkCore;
using SportsPro.DataAccess.Configuration;
using SportsPro.DataAccess.SeedData;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SportsPro.Models;
using Microsoft.AspNetCore.Identity;

namespace SportsPro.DataAccess
{
    public class SportsProContext : IdentityDbContext
    {
        public SportsProContext(DbContextOptions<SportsProContext> options) : base(options) { }

        public DbSet<SportsProUser> SportsProUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //configure many-to-many for Registrations Table.
            modelBuilder.ApplyConfiguration(new RegistrationConfig());
           

            //seed initial data
            modelBuilder.ApplyConfiguration(new SeedCountries());
            modelBuilder.ApplyConfiguration(new SeedCustomers());
            modelBuilder.ApplyConfiguration(new SeedIncidents());
            modelBuilder.ApplyConfiguration(new SeedProducts());
            modelBuilder.ApplyConfiguration(new SeedRegistrations());
            modelBuilder.ApplyConfiguration(new SeedTechnicians());
        }
    }
}
