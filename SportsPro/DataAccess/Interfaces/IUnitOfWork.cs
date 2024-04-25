using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System;
namespace SportsPro.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Customer> Customers { get; }
        IRepository<Country> Countries { get; }
        IRepository<Incident> Incidents { get; }
        IRepository<Product> Products { get; }
        IRepository<Registration> Registrations { get; }
        IRepository<SportsProUser> Technicians { get; }

        IRepository<SportsProUser> SportsProUsers { get; }


        //IIncidentRepository Incidents { get; }
        //ITechnicianRepository Technicians { get; }
        //ICustomerRepository Customers { get; }
        //ICountryRepository Countries { get; }
        //IProductRepository Products { get; }

        //IRegistrationRepository Registrations { get; }

        //IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        void Save();

    }
}
