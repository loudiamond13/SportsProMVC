using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;
using SportsPro.Models.Validations;
using SportsPro.Utility;
using X.PagedList;

namespace SportsPro.Controllers
{
    [Authorize(Roles = RoleConstants.Role_Admin)]
    public class TechnicianController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        // constructor
        public TechnicianController(IUnitOfWork ctx, UserManager<IdentityUser> userManager)
        { 
            _userManager = userManager;
         
            _unitOfWork = ctx;
        }

        //tech list
        [HttpGet]
        public async Task<IActionResult> List(string search, int? page)
        {
            try 
            {
                //set the pageSize
                var pageSize = 5;
                //default the page number to 1 if there is no pagenumber passed in
                var pageNumber = page ?? 1;

                //get all users from the db
                var sportsProUsers = _unitOfWork.SportsProUsers.GetAll();

                //get the role for each user
                foreach (var user in sportsProUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    user.Role = roles.FirstOrDefault();
                    //do not include password
                    user.PasswordHash = null;
                }

                //filter users where Role == "Technician"
                var technicians = sportsProUsers.Where(user => user.Role == RoleConstants.Role_Technician);

                //check if there is techs found
                if (technicians == null)
                {
                    //return to homepage if there is no techs found.
                    TempData["errorMessage"] = "Technicians not found.";
                    return RedirectToAction("Index", "Home");
                }
   
                //filter technicians if search is not null
                if (!string.IsNullOrEmpty(search))
                {
                    technicians = technicians.Where(tech => tech.FirstName.ToLower().Contains(search.ToLower()) ||
                                                    tech.LastName.ToLower().Contains(search.ToLower()) ||
                                                    tech.Email.ToLower().Contains(search.ToLower()) || // search for email as well
                                                    tech.PhoneNumber.Contains(search))            // search for the phone as well
                                                    .OrderBy(tech => tech.FirstName);
                }
             
              
                //paginates the technicians
                var pagedTechnicians = await technicians.ToPagedListAsync(pageNumber, pageSize);

                ViewBag.search = search;

                return View("List", pagedTechnicians);
            }
            catch (Exception ex) 
            {
                TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }



        
        [HttpGet]
        [Route("/technician/edit/id/{id?}")]
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Action = "Edit";

            //find the passed in tech id
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                TempData["errorMessage"] = "Technician not found.";
                return RedirectToAction("List", "Technician");
            }

            return View("AddEdit", user);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(SportsProUser user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //find the technician to be updated
                    var existingTech = _unitOfWork.SportsProUsers.Find(user => user.Id == user.Id);
                    


                    //check if technician is found from the db
                    if (existingTech == null)
                    {
                        TempData["errorMessage"] = $"Technician not found.";
                        return RedirectToAction("List", "Technician");
                    }

                  

                    //check if tech email is new or not matched to the existing technician/user email
                    if (user.Email != existingTech.Email)
                    {
                        //check the new email
                        string message = Validate.CheckIfTechnicianEmailExists(_unitOfWork, user.Email.Trim());
                        //if there is messsage, show the error message by adding it to the model error
                        if (!string.IsNullOrEmpty(message))
                        {
                            ModelState.AddModelError(nameof(SportsProUser.Email), message); // add model error if email is already in use
                            ViewBag.Countries = _unitOfWork.Countries.GetAll().ToList();
                            return View("AddEdit", user);
                        }

                    }

                    //update the following fields if all validation are ok/valid
                    existingTech.FirstName = user.FirstName;
                    existingTech.LastName = user.LastName;
                    existingTech.UserName = user.Email;
                    existingTech.Email = user.Email;
                    existingTech.PhoneNumber = user.PhoneNumber;

                    var result = await _userManager.UpdateAsync(existingTech); // update the user

                    //check the result if it succeeded
                    if (result.Succeeded)
                    {
                        TempData["successMessage"] = "Technician updated successfully";
                        //save the db
                      //  _unitOfWork.Save();
                        return RedirectToAction("List", "Technician");
                    }
                    // else if it was unsuccessful, show the error to the user
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            TempData["errorMessage"] = $"{error.Description}";
                        }
                        return View("AddEdit", user);
                    }
                }
                catch (Exception ex) 
                {
                    TempData["errorMessage"] = $"Error occurred: {ex.Message}"; 
                    return View("AddEdit", user);
                }
            }
            else
            {
                ViewBag.Action = "AddEdit";
                return View("AddEdit", user);
            }
        }


        //lock/unlock action
     
        public async Task<IActionResult> UnlockLock(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    TempData["errorMessage"] = "User to unlock/lock not found.";
                    return View("List");
                }

               

                //check if user is currently locked
                if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
                {
                    //unlock the user
                    //set the lockout end to now
                    user.LockoutEnd = DateTime.Now;
                    TempData["successMessage"] = "User successfully unlocked.";
                }
                //else lock the user
                else 
                {
                    user.LockoutEnd = DateTime.Now.AddYears(100);
                    TempData["successMessage"] = "User successfully locked.";
                }

                var result = await _userManager.UpdateAsync(user);

                //check if user is updated successfully
                if (result.Succeeded)
                {
                    return RedirectToAction("List", "Technician");
                }
                else 
                {
                    TempData["errorMessage"] = "Error on locking/locking user.";
                    return RedirectToAction("List", "Technician");
                }

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                return View("List");
            }

        }


        [HttpPost]
        public async Task<IActionResult> Delete(string technicianID)
        {
            try 
            {
                //get the technician from the db using the technicianID that is passed in
                var userToDelete = await _userManager.FindByIdAsync(technicianID);

                //check if technician exist
                if (userToDelete == null) {
                    TempData["errorMessage"] = "Technician not found.";
                    return RedirectToAction("List", "Technician");
                }

                // Delete the user
                var result = await _userManager.DeleteAsync(userToDelete);

                if (result.Succeeded)
                {
                    TempData["successMessage"] = "User deleted successfully";
                }
                else
                {
                    TempData["errorMessage"] = "Failed to delete user";
                }

                // _unitOfWork.Save(); //save the db
                return RedirectToAction("List", "Technician");
                
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
