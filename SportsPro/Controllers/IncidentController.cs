using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;
using SportsPro.Models.ViewModels;
using SportsPro.Utility;
using X.PagedList;

namespace SportsPro.Controllers
{
	[Authorize]
    public class IncidentController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<IdentityUser> _userManager;

		public IncidentController(IUnitOfWork ctx, UserManager<IdentityUser> userManager)
		{
			_userManager = userManager;
			_unitOfWork = ctx;
		}

		

		public async Task IncidentsListInfo()
		{
			
			//store products in a viewbag,ordered by name
			ViewBag.Products =  _unitOfWork.Products.GetAll().OrderBy(prod => prod.Name).ToList();

			//get all users for tech list
			var users = _unitOfWork.SportsProUsers.GetAll().OrderBy(user => user.FirstName).ToList();

			//for each user in users get each role
			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				user.Role = roles.FirstOrDefault();
				//do not include hashed password
				user.PasswordHash = null;
			}
			//store techs in a viewbag, ordered by name
			ViewBag.Technicians = users.Where(user => user.Role == RoleConstants.Role_Technician).ToList();

			//store customers in a viewbag order by name														
			ViewBag.Customers = _unitOfWork.Customers.GetAll().OrderBy(cust => cust.FirstName).ToList();
		}


		[Authorize(Roles = RoleConstants.Role_Admin)]
		//incident list
		[HttpGet]
		public async Task<IActionResult> List(string search, int? page, bool? unassigned, bool? open)
		{
			try
			{
                //set the pageSize
                var pageSize = 5;
                var pageNumber = page ?? 1; // default the pageNumber to 1 if there is no pagenumber passed in


                //get all incident
                //include Customer,Product, SportsProUser, and DateOpened
                var incidents = _unitOfWork.Incidents.GetAll(includeProperties: "Customer,Product,Technician");


                if (!string.IsNullOrEmpty(search))
                {
                    incidents = incidents.Where(incident => incident.Title.ToLower().Contains(search.ToLower()) ||  // search for the title
                                                            incident.Description.ToLower().Contains(search.ToLower()) || //search for the description
                                                            incident.Product.Name.ToLower().Contains(search.ToLower()) || //search for the product name
                                                            incident.Customer.FirstName.ToLower().Contains(search.ToLower()) || //search for the customer firstname
                                                            incident.Customer.LastName.ToLower().Contains(search.ToLower())); // search for the customer lastname
                }

                // check if unassigned filter is clicked/active for incidents
                if (unassigned == true)
                {
                    incidents = incidents.Where(incident => incident.TechnicianID == null);
                }

                //check if open incidents is clicked/active
                if (open == true)
                {
                    incidents = incidents.Where(incident => incident.DateClosed == null);
                }

                //paginates the items using X.PagedList package
                var pagedIncidents = await incidents.ToPagedListAsync(pageNumber, pageSize);

                ViewBag.search = search;

                return View("List", pagedIncidents);
            }
			catch(Exception ex) 
			{
				TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("Index", "Home");
			}
		}

        [Authorize(Roles = RoleConstants.Role_Admin)]
        // update incident list 
        //technician select list
        [Route("update-incident/select-tech/")]
		[HttpGet]
        public async Task<IActionResult> TechniciansSelectList()
        {
			try 
			{
				//get all tech
				await IncidentsListInfo();

                // returns the technicians
                return View("TechniciansSelectList");
            }
			catch (Exception ex)
			{
				TempData["errorMessage"] = $"Error occurred: {ex.Message}";
				return RedirectToAction("Index", "Home");
			}
        }



		// list of the incidents that is associeted with the selected technician
		[Route("/update-incident/list/id/{id?}")]
		[HttpGet]
        public async Task<IActionResult> UpdateIncidentsList(string technicianID, int? page)
        {
			try
			{
                //set the pageSize
                var pageSize = 5;
                var pageNumber = page ?? 1; // default the pageNumber to 1 if there is no pagenumber passed in
                                            


                List<Incident> incidents = _unitOfWork.Incidents.GetAll(includeProperties: "Customer,Technician,Product")
                                            .Where(incident => incident.TechnicianID == technicianID)
                                                   .ToList();

				SportsProUser technician = _unitOfWork.SportsProUsers.Find(tech => tech.Id == technicianID) ;
				ViewBag.Technician = technician.FullName;

				if (incidents.Count() == 0)
				{
					TempData["errorMessage"] = $"Technician {technician.FullName} Has No Open Incidents.";
					await IncidentsListInfo();
					return View("TechniciansSelectList");
				}

				var pagedIncidents = incidents.ToPagedList(pageNumber, pageSize);

                return View("UpdateIncidentsList", pagedIncidents);
            }
			catch(Exception ex)
			{
				TempData["errorMessage"] = $"Error occurred: {ex.Message}";
				return RedirectToAction("Index", "Home");
			}
        }

		
        [Route("update-incident/update/id/{id?}")]
        [HttpGet]
        public IActionResult UpdateIncident(int incidentID, string technicianID)
        {
			
            ViewBag.Action = "Update Incident";
            var incident = _unitOfWork.Incidents
                            .Find(inci => inci.IncidentID == incidentID, includeProperties: "Technician,Customer,Product");

			//check if the selected incident is in the db
			if (incident == null) 
			{
				//send error message if no incident found.
				TempData["errorMessage"] = "Incident to update not found.";
				return RedirectToAction("UpdateIncidentsList", new { technicianID = technicianID});
			}

            return View("UpdateIncident", incident);
            
        }

		
		[HttpPost]
		public IActionResult Save(Incident incident)
		{
			if (ModelState.IsValid)
			{

				//check if incident has id to update
				if (incident.IncidentID != 0)
				{
					try
					{
						//to be fixed
						var existingIncident = _unitOfWork.Incidents
											.Find(incident => incident.IncidentID == incident.IncidentID, includeProperties: "Product,Technician,Customer");

						//double check if incident exist in db
						if (existingIncident == null)
						{
							TempData["errorMessage"] = "Unable to find incident to update.";
							return View("UpdateIncident", incident);
						}
						//else if the incident exists, update the incident
						else
						{
                            existingIncident.ProductID = incident.ProductID;
                            existingIncident.CustomerID = incident.CustomerID;
                            existingIncident.DateClosed = incident.DateClosed;
                            existingIncident.DateOpened = incident.DateOpened;
                            existingIncident.Title = incident.Title;
                            existingIncident.Description = incident.Description;
                            existingIncident.TechnicianID = incident.TechnicianID;
                            TempData["successMessage"] = "Incident updated successfully.";
						}
						//save the data
						_unitOfWork.Save();

						//if the user who edit this incident is tech, return to home
						//if an admin edits this incident, return to techlist
						if (User.IsInRole(RoleConstants.Role_Admin))
						{
							return RedirectToAction("UpdateIncidentsList", new { technicianID = existingIncident.TechnicianID });
						}
						else 
						{
							return RedirectToAction("Index", "Home");
						}
					}
					catch (Exception ex)
					{
						TempData["errorMessage"] = $"Error occurred: {ex.Message}";
						return View("UpdateIncident", incident);
					}
				}
                //else if the incident has no id/ id == 0, return to select tech
                else
                {
					TempData["errorMessage"] = "Incident has no Id.";
					return View("UpdateIncident", incident);
				}
			}
			else
			{
               
                var existingIncident = _unitOfWork.Incidents
                                    .Find(incident => incident.IncidentID == incident.IncidentID, includeProperties: "Technician,Product,Customer");

                TempData["errorMessage"] = "Please input the required fields.";
                return View("UpdateIncident", existingIncident);
			}
		}


        [Authorize(Roles = RoleConstants.Role_Admin)]
        [Route("/incident/add/")]
        [HttpGet]
		public async Task<IActionResult> Add()
		{
			ViewBag.Action = "Add";

			await IncidentsListInfo();

            return View("AddEdit", new Incident());
		}

        [Authorize(Roles = RoleConstants.Role_Admin)]
        [Route("/incidents/edit/id/{id?}")]
		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			try 
			{
                ViewBag.Action = "Edit";

               await IncidentsListInfo();

                //find and get the incident from the db 
                var incident = _unitOfWork.Incidents.Find(incident => incident.IncidentID == id);


                return View("AddEdit", incident);
            }
			catch (Exception ex) 
			{
				TempData["errorMessage"] = $"Error occurred: {ex.Message}";
				return RedirectToAction("List", "Incident");
			}
		}


        [Authorize(Roles = RoleConstants.Role_Admin)]
        [HttpPost]
		public async Task<IActionResult> AddEdit(Incident incident)
		{

			//check if inputs are valid
			if (ModelState.IsValid)
			{
				try
				{
                    // if inputs are valid and there is incident id/choosen, update it
                    if (incident.IncidentID != 0)
                    {
						//get this incident from the db
                        var existingIncident = _unitOfWork.Incidents.Find(i => i.IncidentID == incident.IncidentID);

                        if (existingIncident != null)
                        {
							//update the following fields
                            existingIncident.ProductID = incident.ProductID;
                            existingIncident.CustomerID = incident.CustomerID;
                            existingIncident.DateClosed = incident.DateClosed;
                            existingIncident.DateOpened = incident.DateOpened;
                            existingIncident.Title = incident.Title;
                            existingIncident.Description = incident.Description;
                            existingIncident.TechnicianID = incident.TechnicianID;

                            TempData["successMessage"] = $"Incident updated successfully.";
                        }
                        else
                        {
                            TempData["errorMessage"] = $"Error on getting Incident";
                        }
                    }
                    //else add a new incident ( incident id == 0 )
                    else
                    {
                        _unitOfWork.Incidents.Add(incident);
                        TempData["successMessage"] = $"Incident added successfully.";
                    }

                    //save the changes
                    _unitOfWork.Save();


                    return RedirectToAction("List", "Incident");
                }
				catch (Exception ex) 
				{
					TempData["errorMessage"] = $"Error occurred: {ex.Message}";
					return View("AddEdit", incident);
				}
                
            }
			else
			{

				await IncidentsListInfo();

				//  if inputs are not valid, do not go anywhere
				return View("AddEdit", incident);

			}
		}


        [Authorize(Roles = RoleConstants.Role_Admin)]
        [HttpPost]
		public IActionResult Delete(int incidentID)
		{
			try
			{
				Incident incident = _unitOfWork.Incidents.Find(inci => inci.IncidentID == incidentID);

				_unitOfWork.Incidents.Delete(incident); // deletes the choosen incident
				TempData["successMessage"] = "Incident deleted successfully";
				_unitOfWork.Save();    // save changes

				return RedirectToAction("List", "Incident"); // return to the List View of Incident
			}
			catch (Exception ex) 
			{
				TempData["errorMessage"] = $"Error occurred: {ex.Message}";
				return RedirectToAction("List", "Incident");
			}
		}



    }
}
