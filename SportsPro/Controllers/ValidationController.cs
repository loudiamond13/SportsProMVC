using Microsoft.AspNetCore.Mvc;
using SportsPro.DataAccess;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models.Validations;

namespace SportsPro.Controllers
{
    public class ValidationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValidationController(IUnitOfWork context)
        {
            _unitOfWork = context;
        }

        public JsonResult ValidateEmail (string email)
        {
          
            string message = "asd";//Validate.CheckIfEmailExists(_unitOfWork, email);
            if (!string.IsNullOrEmpty(message))
            { 
                return Json(message);
            }
            

            TempData["okEmail"] = true;
            return Json(true);
        }
    }
}
