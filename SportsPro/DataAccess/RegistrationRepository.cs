using Microsoft.EntityFrameworkCore;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;

namespace SportsPro.DataAccess
{
    public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
       private readonly SportsProContext _context;
        public RegistrationRepository(SportsProContext ctx) : base(ctx) => _context = ctx;

       
    }
}
