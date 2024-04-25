using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;
using SportsPro.Models.ViewModels;
using SportsPro.Utility;


namespace SportsPro.Controllers
{
    [Authorize(Roles = RoleConstants.Role_Admin)]
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
            try
            {
                //get all customers and store them to Viewbag
                ViewBag.Customers = _unitOfWork.Customers.GetAll().OrderBy(c => c.LastName).ToList();

                //int custID = HttpContext.Session.GetInt32("custID") ?? 0;
                Customer customer = new Customer();

                //if (custID == 0)
                //{ 
                //    customer = new Customer();
                //}
                //else 
                //{
                //    customer = _unitOfWork.Customers.Find(customer => customer.CustomerID == custID);

                //}

                return View("GetCustomer", customer);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        [Route("[controller]s/customerID/{customerID}")]
        public IActionResult List(int customerID)
        {
            try
            {
                if (customerID == 0)
                {
                    TempData["errorMessage"] = "You Must Select A Customer.";
                    return RedirectToAction("GetCustomer", "Registration");
                }
                else 
                {
                    RegistrationViewModel viewModel = new RegistrationViewModel()
                    {
                        Customer = _unitOfWork.Customers.Find(customer => customer.CustomerID == customerID),
                        Products = _unitOfWork.Products.GetAll().ToList(),
                        Registrations = _unitOfWork.Registrations.GetAll(includeProperties: "Product,Customer")
                                                      .Where(r => r.CustomerID == customerID)
                                                      .ToList()
                    };

                    if (viewModel.Customer == null) 
                    {
                        TempData["errorMessage"] = "Unable to find the customer";
                        return RedirectToAction("GetCustomer", "Registration");
                    }

                    return View("List", viewModel);
                }
               
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("GetCustomer", "Registration");
            }
        }

        [HttpPost]
        [Route("[controller]s")]
        public IActionResult List(Customer customer)
        {
            //HttpContext.Session.SetInt32("custID", customer.CustomerID);

            if (customer.CustomerID == 0)
            {
                TempData["errorMessage"] = "You Must Select A Customer.";
                return RedirectToAction("GetCustomer", "Registration");
            }
            else
            {
                return RedirectToAction("List", new { customerID = customer.CustomerID });
            }
        }

        [HttpPost]
        public IActionResult Register(RegistrationViewModel viewModel)
        {
            if (viewModel.ProductID == 0)
            {
                TempData["errorMessage"] = "You Must Select A Product.";
                return RedirectToAction("List", new { customerID = viewModel.CustomerID });
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

                    //check if this product to register is already register
                    if (message.Contains("duplicate key"))
                    {
                        //sent error message if this product is already registered to this customer
                        TempData["errorMessage"] = $"This Product Is Already Registered To This Customer.";
                    }
                    else
                    {
                        TempData["errorMessage"] = $"Error Accessing Database: {message}";
                    }
                    
                }
                
            }
            return RedirectToAction("List", new { customerID = viewModel.CustomerID });
        }

    
        //delete action
        [HttpPost]
        public IActionResult Delete(int registrationID, int customerID) 
        {
            try
            {
                //get the registration from the db
                Registration registration = _unitOfWork.Registrations
                                           .Find(registration => registration.RegistrationID == registrationID, includeProperties: "Product");

                //check if the registration is found in db
                if (registration == null) 
                {
                    //if registration do not exists show error message
                    TempData["errorMessage"] = "Registration not found.";
                    return RedirectToAction("List", new { customerID = customerID });
                }

                
                TempData["successMessage"] = $"Product '{registration.Product.Name}' Has Been Successfully Deleted.";

                _unitOfWork.Registrations.Delete(registration);
                _unitOfWork.Save();


                return RedirectToAction("List", new { customerID = customerID });
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("List", new {customerID = customerID});
            }
        }

    }
}
