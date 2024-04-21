using Microsoft.AspNetCore.Mvc;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;
using SportsPro.Models.Validations;
using X.PagedList;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // constructor
        public TechnicianController(IUnitOfWork ctx)
        { 
            
            _unitOfWork = ctx;
        }

        [Route("/technicians")]
        [HttpGet]
        public async Task<IActionResult> List(string search, int? page)
        {
            //set the pageSize
            var pageSize = 5;
            //default the page number to 1 if there is no pagenumber passed in
            var pageNumber = page ?? 1;

            //get all techs from the db
            var technicians = _unitOfWork.Technicians.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                technicians = technicians.Where(tech => tech.FirstName.ToLower().Contains(search.ToLower()) ||
                                                tech.LastName.ToLower().Contains(search.ToLower()) ||
                                                tech.Email.ToLower().Contains(search.ToLower()) || // search for email as well
                                                tech.Phone.Contains(search))            // search for the phone as well
                                                .OrderBy(tech => tech.FirstName);
            }


            var pagedTechnicians =  await technicians.ToPagedListAsync(pageNumber, pageSize);

            ViewBag.search = search;

            return View("List", pagedTechnicians);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";

            //return the AddEdit View With a new/empty techs context
            return View("AddEdit", new Technician());
        }


        
        [HttpGet]
        [Route("/technicians/edit/id/{id?}")]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";

            //find the passed in tech id
            var technician = _unitOfWork.Technicians.Find(tech => tech.TechnicianID == id);

            //  
            return View("AddEdit", technician);
        }

        [HttpPost]
        public IActionResult AddEdit(Technician technician)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (technician.TechnicianID == 0)
                    {
                        string message = Validate.CheckIfTechnicianEmailExists(_unitOfWork, technician.Email.Trim());
                        if (!string.IsNullOrEmpty(message))
                        {
                            ModelState.AddModelError(nameof(Technician.Email), message); // add model error if email is already in use
                            return View("AddEdit", technician);
                        }
                        else 
                        {
                            _unitOfWork.Technicians.Add(technician);
                            TempData["successMessage"] = "Technician added successfully.";
                        }
                    }
                    else
                    {
                        //find the technician to be updated
                        var existingTech = _unitOfWork.Technicians.Find(tech => tech.TechnicianID == technician.TechnicianID);


                        //check if technician is found from the db
                        if (existingTech != null)
                        {
                            //check if tech email is new or not matched to the existing technician email
                            if (technician.Email != existingTech.Email)
                            {
                                //check the new email
                                string message = Validate.CheckIfTechnicianEmailExists(_unitOfWork, technician.Email.Trim());
                                if (!string.IsNullOrEmpty(message))
                                {
                                    ModelState.AddModelError(nameof(Customer.Email), message); // add model error if email is already in use
                                    ViewBag.Countries = _unitOfWork.Countries.GetAll().ToList();
                                    return View("AddEdit", technician);
                                }
                            }
                            

                            //update the following fields if all validation are ok/valid
                            existingTech.FirstName = technician.FirstName;
                            existingTech.LastName = technician.LastName;
                            existingTech.Email = technician.Email;
                            existingTech.Phone = technician.Phone;

                            TempData["successMessage"] = "Technician updated successfully";
                        }
                        else 
                        {
                            TempData["errorMessage"] = $"Technician not found.";
                            return RedirectToAction("List", "Technician");
                        }
                    }

                    _unitOfWork.Save();
                }
                catch (Exception ex) 
                {
                    TempData["errorMessage"] = $"Error occurred: {ex.Message}"; 
                }
                return RedirectToAction("List", "Technician");
            }
            else
            {
                ViewBag.Action = technician.TechnicianID == 0 ? "Add" : "Edit";
                return View("AddEdit", technician);
            }
        }


        [HttpPost]
        public IActionResult Delete(int technicianID)
        {
            try 
            {
                //get the technician from the db using the technicianID that is passed in
                Technician technician = _unitOfWork.Technicians.Find(tech => tech.TechnicianID == technicianID); 

                //check if technician exist
                if (technician == null) {
                    TempData["errorMessage"] = "Technician not found.";
                    return RedirectToAction("List", "Technician");
                }

                _unitOfWork.Technicians.Delete(technician); // delete the technician
                _unitOfWork.Save(); //save the db
                
            }
            catch (Exception ex) 
            {
                //show error message
                TempData["errorMessage"] = $"Error occurred: {ex.Message}";
            }
            return RedirectToAction("List", "Technician");
        }

    }
}
