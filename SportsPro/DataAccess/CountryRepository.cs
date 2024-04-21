using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;

namespace SportsPro.DataAccess
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly SportsProContext _context;

        public CountryRepository(SportsProContext ctx) : base(ctx) => _context = ctx;
    }
}
