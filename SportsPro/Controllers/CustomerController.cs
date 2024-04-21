using Microsoft.AspNetCore.Mvc;
using SportsPro.DataAccess;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;
using SportsPro.Models.Validations;
using X.PagedList;

namespace SportsPro.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //constructor
        public CustomerController(IUnitOfWork ctx)
        { 
            _unitOfWork = ctx;
        }

        [Route("/customers")]
        [HttpGet]
        public async Task<IActionResult> List(string search, int? page)
        {
            try {
                //setting the pagesize
                var pageSize = 5;
                var pageNumber = page ?? 1; //defaults the pageNumber to 1 if there is no pagenumber passed in

                var customers = _unitOfWork.Customers.GetAll(includeProperties: "Country")
                                                     .OrderBy(cust => cust.FirstName)
                                                     .ToList();

                //check if there is customer/s
                if (customers == null)
                {
                    TempData["errorMessage"] = "Error on getting the customer.";
                    return View("Index");
                }

                if (!string.IsNullOrEmpty(search))
                {
                    //search for first name, last name phone. email, and city
                    //make sure to lower all to be case-insensitive
                    customers = customers.Where(customer => customer.FirstName.ToLower().Contains(search.ToLower()) ||
                                                customer.LastName.ToLower().Contains(search.ToLower()) ||
                                                customer.Phone.ToLower().Contains(search.ToLower()) ||
                                                customer.Email.ToLower().Contains(search.ToLower()) ||
                                                customer.City.ToLower().Contains(search.ToLower())).ToList();
                }

                var pagedCustomers = await customers.ToPagedListAsync(pageNumber, pageSize);

                ViewBag.search = search;

                return View("List", pagedCustomers);
            }
            catch (Exception ex) 
            {
                TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Add() 
        {
            ViewBag.Action = "Add";
            ViewBag.Countries = _unitOfWork.Countries.GetAll().ToList();

            return View("AddEdit",new Customer());
        }

        [Route("/customers/edit/id/{id?}")]
        [HttpGet]
        public IActionResult Edit(int id) 
        {
            try 
            {
                ViewBag.Countries = _unitOfWork.Countries.GetAll().ToList();
                ViewBag.Action = "Edit";

                //find and get the customer from db
                var customer = _unitOfWork.Customers.Find(customer => customer.CustomerID == id);

                //check if customer exists 
                if (customer == null)
                {
                    //show error message if customer is not found.
                    TempData["errorMessage"] = "Error on getting the customer.";
                    return RedirectToAction("List", "Customer");
                }

                return View("AddEdit", customer);
            }
            //show error message if there is any other error occurred.
            catch (Exception ex) 
            {
                TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("List", "Customer");
            }
        }

        //add/edit post request
        [HttpPost]
        public IActionResult AddEdit(Customer customer) 
        {

            if (ModelState.IsValid) // check if model state is valid
            {
                try
                {
                    //check if customer has an id
                    if (customer.CustomerID != 0)
                    {
                        //if customer has an id
                        //find the customer in the db
                        var existingCustomer = _unitOfWork.Customers.Find(cust => cust.CustomerID == customer.CustomerID);

                        //check if customer found from the db
                        if (existingCustomer != null)
                        {
                            //check if the customer to be updated email and the updated customer email matched, if email don't match, check if the new email is in used 
                            if (customer.Email != existingCustomer.Email)
                            {
                                //check the new email
                                string message = Validate.CheckIfCustomerEmailExists(_unitOfWork, customer.Email.Trim());
                                if (!string.IsNullOrEmpty(message))
                                {
                                    ModelState.AddModelError(nameof(Customer.Email), message); // add model error if email is already in use
                                    ViewBag.Countries = _unitOfWork.Countries.GetAll().ToList();
                                    return View("AddEdit", customer);
                                }
                            }

                            //if customer exists, and all validation are ok/valid update the fields
                            existingCustomer.FirstName = customer.FirstName;
                            existingCustomer.LastName = customer.LastName;
                            existingCustomer.Email = customer.Email;
                            existingCustomer.Phone = customer.Phone;
                            existingCustomer.City = customer.City;
                            existingCustomer.CountryID = customer.CountryID;
                            existingCustomer.State = customer.State;
                            existingCustomer.Address = customer.Address;
                            existingCustomer.PostalCode = customer.PostalCode;

                            TempData["successMessage"] = "Successfully updated customer.";
                        }
                        else
                        {
                            TempData["errorMessage"] = "Customer not found.";
                        }
                    }
                    else
                    {
                        //check the new email
                        string message = Validate.CheckIfCustomerEmailExists(_unitOfWork, customer.Email.Trim());
                        if (!string.IsNullOrEmpty(message))
                        {
                            ModelState.AddModelError(nameof(Customer.Email), message); // add model error if email is already in use
                            ViewBag.Countries = _unitOfWork.Countries.GetAll().ToList();
                            return View("AddEdit", customer);
                        }
                        else
                        {
                            // add the new customer
                            _unitOfWork.Customers.Add(customer);
                            TempData["successMessage"] = "Customer added successfully.";
                        }
                    }

                    //save the customer
                    _unitOfWork.Save();
                    //redirect
                    return RedirectToAction("List", "Customer");
                }
                catch (Exception ex) 
                {
                    TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                    return RedirectToAction("List", "Customer");
                }
            }
            else 
            {
                TempData["errorMessage"] = "Please input the required fields.";
                ViewBag.Action = customer.CustomerID == 0 ? "Add" : "Edit";
                ViewBag.Countries = _unitOfWork.Countries.GetAll().ToList();
                return View("AddEdit", customer);
            }
        
        }

       
        //delete action
        [HttpPost]
        public IActionResult Delete(int customerID) 
        {
            try
            {
                Customer customer = _unitOfWork.Customers.Find(cust => cust.CustomerID == customerID);

                //double check if customer exist
                if (customer == null)
                {
                    TempData["errorMessage"] = "Customer not found";
                    return RedirectToAction("List", "Customer");
                }

                _unitOfWork.Customers.Delete(customer);
                _unitOfWork.Save();

                TempData["successMessage"] = "Customer deleted successfully.";
                return RedirectToAction("List", "Customer");
            }
            catch (Exception ex) 
            {
                TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("List", "Customer");
            }
        }
    }
}
