using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsPro.DataAccess.Interfaces;
using System.Security.Claims;
using X.PagedList;

namespace SportsPro.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int? page)
        {
            //initialize the page size
            int pageSize = 5;
            int pageNumber = page ?? 1; // default the page to page 1

            //gets the current user id.
            string userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           
            var incidents = _unitOfWork.Incidents.GetAll(includeProperties: "Customer,Product").Where(incident => incident.TechnicianID == userID);

            var pagedIncidents = incidents.ToPagedList(pageNumber,pageSize);

            int asd = 1 + 1; 
            return View(pagedIncidents);
        }


        [Route("/about")]
        public IActionResult About() 
        {
            return View();
        }

       
    }
}