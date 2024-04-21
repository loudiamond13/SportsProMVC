using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;

namespace SportsPro.DataAccess
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly SportsProContext _context;
        public ProductRepository(SportsProContext ctx) : base(ctx) => _context = ctx;
    }



}
