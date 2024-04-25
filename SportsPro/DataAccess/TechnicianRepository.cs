using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;

namespace SportsPro.DataAccess
{
    public class TechnicianRepository : Repository<Technician>, ITechnicianRepository
    {
        private readonly SportsProContext _context;

        public TechnicianRepository(SportsProContext ctx) : base(ctx) => _context = ctx;

    }
}
