
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsPro.Models;

namespace SportsPro.DataAccess.SeedData
{
    internal class SeedTechnicians : IEntityTypeConfiguration<Technician>
    {
        public void Configure(EntityTypeBuilder<Technician> entity)
        {
            entity.HasData(
            new Technician
            {
                TechnicianID = 11,
                FirstName = "Alison",
                LastName = "Diaz",
                Email = "alison@sportsprosoftware.com",
                Phone = "800-555-0443"
            },
                new Technician
                {
                    TechnicianID = 12,
                    FirstName = "Jason",
                    LastName = "Lee",
                    Email = "jason@sportsprosoftware.com",
                    Phone = "800-555-0444"
                },
                new Technician
                {
                    TechnicianID = 13,
                    FirstName = "Andrew",
                    LastName = "Wilson",
                    Email = "awilson@sportsprosoftware.com",
                    Phone = "800-555-0449"
                },
                new Technician
                {
                    TechnicianID = 14,
                    FirstName = "Gunter",
                    LastName = "Wendt",
                    Email = "gunter@sportsprosoftware.com",
                    Phone = "800-555-0400"
                },
                new Technician
                {
                    TechnicianID = 15,
                    FirstName = "Gina",
                    LastName = "Fiori",
                    Email = "gfiori@sportsprosoftware.com",
                    Phone = "800-555-0459"
                }

                );
        }

    }
}
