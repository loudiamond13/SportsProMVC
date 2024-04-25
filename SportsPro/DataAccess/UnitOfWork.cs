using SportsPro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SportsPro.DataAccess.Interfaces;

namespace SportsPro.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SportsProContext _dbContext;
        //public UnitOfWork(SportsProContext ctx)
        //{
        //    _dbContext = ctx;
        //    Incidents = new IncidentRepository(_dbContext);
        //    Technicians = new TechnicianRepository(_dbContext);
        //    Customers = new CustomerRepository(_dbContext);
        //    Registrations = new RegistrationRepository(_dbContext);
        //    Products = new ProductRepository(_dbContext);
        //    Countries = new CountryRepository(_dbContext);
        //}

        //public IIncidentRepository Incidents { get; private set; }
        //public ITechnicianRepository Technicians { get; private set; }
        //public ICustomerRepository Customers { get; private set; }
        //public IRegistrationRepository Registrations { get; private set; }
        //public IProductRepository Products { get; private set; }

        //public ICountryRepository Countries { get; private set; }

        //public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        //{
        //    return new Repository<TEntity>(_dbContext);
        //}

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }


        public IRepository<SportsProUser> SportsProUsers { get; private set; }
        public IRepository<Incident> Incidents { get; private set; }
        public IRepository<SportsProUser> Technicians { get; private set; }
        public IRepository<Customer> Customers { get; private set; }
        public IRepository<Registration> Registrations { get; private set; }

        public IRepository<Product> Products { get; private set; }
        public IRepository<Country> Countries { get; private set; }

        public UnitOfWork(SportsProContext dbContext)
        {
            _dbContext = dbContext;
            Incidents = new Repository<Incident>(_dbContext);
            Technicians = new Repository<SportsProUser>(_dbContext);
            Customers = new Repository<Customer>(_dbContext);
            Registrations = new Repository<Registration>(_dbContext);
            Products = new Repository<Product>(_dbContext);
            Countries = new Repository<Country>(_dbContext);
            SportsProUsers = new Repository<SportsProUser>(_dbContext); 
    
        }
    }
}
