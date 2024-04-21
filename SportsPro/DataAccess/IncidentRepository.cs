using SportsPro.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SportsPro.DataAccess.Interfaces;

namespace SportsPro.DataAccess
{
    public class IncidentRepository : Repository<Incident>, IIncidentRepository
    {
        private readonly SportsProContext _context;
        public IncidentRepository(SportsProContext ctx) : base(ctx) => _context = ctx;


        //public IEnumerable<Incident> GetAllOpenIncidents()
        //{
        //    return _context.Incidents.Where(i => i.DateClosed == null)
        //                                     .Include(incident => incident.Customer) // include customer associated to this incident
        //                                     .Include(incident => incident.Product) // include product as well
        //                                     .ToList(); // to list
        //}

      

        //public IEnumerable<Incident> GetIncidentsOfSelectedTech(int id)
        //{




        //    return _context.Incidents.Include(i => i.Technician)
        //                                        .Include(i => i.Product)
        //                                        .Include(i => i.Customer)
        //                                    .Where(i => i.TechnicianID == id && i.DateClosed == null)
        //                                    .ToList();
        //}

      
    }
}
