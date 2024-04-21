using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsPro.Models;

namespace SportsPro.DataAccess.SeedData
{
    internal class SeedRegistrations : IEntityTypeConfiguration<Registration>
    {

        public void Configure(EntityTypeBuilder<Registration> entity)
        {
            entity.HasData(
                new Registration
                {
                    RegistrationID = 1,
                    CustomerID = 1002,
                    ProductID = 1,
                    RegistrationDate = DateTime.Parse("2017-02-01")
                },

                new Registration
                {
                    RegistrationID = 2,
                    CustomerID = 1002,
                    ProductID = 3,
                    RegistrationDate = DateTime.Parse("2017-02-01")
                },

                new Registration
                {
                    RegistrationID = 3,
                    CustomerID = 1010,
                    ProductID = 2,
                    RegistrationDate = DateTime.Parse("2017-02-01")
                }
                ) ;
        }
    }
}
