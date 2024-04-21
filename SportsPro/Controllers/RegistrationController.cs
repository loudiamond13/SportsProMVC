using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.DataAccess;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;
using SportsPro.Models.ViewModels;
using System.Linq;

namespace SportsPro.Controllers
{ 
    public class RegistrationController : Controller
    {
       private readonly IUnitOfWork _unitOfWork;

        public RegistrationController(IUnitOfWork ctx)
        {
            _unitOfWork = ctx;
        }

        [HttpGet]
        public IActionResult GetCustomer()
        {
            ViewBag.Customers = _unitOfWork.Customers.GetAll().OrderBy(c => c.LastName).ToList();

            int custID = HttpContext.Session.GetInt32("custID") ?? 0;
            Customer customer;

            if (custID == 0)
            { 
                customer = new Customer();
            }
            else 
            {
                customer = _unitOfWork.Customers.Find(customer => customer.CustomerID == custID);
               
            }

            return View(customer);
        }


        [HttpGet]
        [Route("[controller]s/id/{id}")]
        public IActionResult List(int id)
        {
            RegistrationViewModel viewModel = new RegistrationViewModel()
            {
                Customer = _unitOfWork.Customers.Find(customer => customer.CustomerID == id),
                Products = _unitOfWork.Products.GetAll().ToList(),
                Registrations = _unitOfWork.Registrations.GetAll(includeProperties: "Product,Customer")
                                                        .Where(r => r.CustomerID == id)
                                                        .ToList()
            };

            return View(viewModel); 
        }

        [HttpPost]
        [Route("[controller]s")]
        public IActionResult List(Customer customer)
        {
            HttpContext.Session.SetInt32("custID", customer.CustomerID);

            if (customer.CustomerID == 0)
            {
                TempData["errorMessage"] = "You Must Select A Customer.";
                return RedirectToAction("GetCustomer");
            }
            else
            {
                return RedirectToAction("List", new { id = customer.CustomerID });
            }
        }

        [HttpPost]
        public IActionResult Register(RegistrationViewModel viewModel)
        {
            if (viewModel.ProductID == 0)
            {
                TempData["errorMessage"] = "You Must Select A Product.";
            }
            else 
            {
                try
                {
                    Registration registration = new Registration()
                    {
                        CustomerID = viewModel.CustomerID,
                        ProductID = viewModel.ProductID,
                    };

                    _unitOfWork.Registrations.Add(registration);
                    _unitOfWork.Save();
                }
                catch(DbUpdateException ex)
                {
                    string message = (ex.InnerException == null) ?
                                        ex.Message : ex.InnerException.Message.ToString();

                    if (message.Contains("duplicate key"))
                    {
                        TempData["errorMessage"] = $"This Product Is Already Registered To This Customer.";
                    }
                    else
                    {
                        TempData["errorMessage"] = $"Error Accessing Database: {message}";
                    }
                    
                }
                
            }
            return RedirectToAction("List", new { id = viewModel.CustomerID });
        }

    
        [HttpPost]
        public IActionResult Delete(int productID, int registrationID, int customerID) 
        {
            Product product = _unitOfWork.Products.Find(product => product.ProductID == productID);
            
           
            Registration registration = _unitOfWork.Registrations.Find(registration => registration.RegistrationID == registrationID);
          
            TempData["successMessage"] = $"Product '{product.Name}' Has Been Successfully Deleted.";

            _unitOfWork.Registrations.Delete(registration);
            _unitOfWork.Save();
           
            
            return RedirectToAction("List", new { id = customerID});
        }

    }
}
