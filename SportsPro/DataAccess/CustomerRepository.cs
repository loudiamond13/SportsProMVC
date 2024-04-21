using Microsoft.EntityFrameworkCore;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;

namespace SportsPro.DataAccess
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly SportsProContext _context;
        public CustomerRepository(SportsProContext ctx) : base(ctx) => _context = ctx;
    }
}
