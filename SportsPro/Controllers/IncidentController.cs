using Microsoft.AspNetCore.Mvc;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;
using SportsPro.Models.ViewModels;
using X.PagedList;

namespace SportsPro.Controllers
{
    public class IncidentController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public IncidentController(IUnitOfWork ctx)
		{
			_unitOfWork = ctx;
		}

		

		public void IncidentsListInfo()
		{
			//store products in a viewbag,ordered by name
			ViewBag.Products =  _unitOfWork.Products.GetAll().OrderBy(prod => prod.Name).ToList();

			//store techs in a viewbag, ordered by name
			ViewBag.Technicians = _unitOfWork.Technicians.GetAll().OrderBy(tech => tech.FirstName).ToList();

			//store customers in a viewbag order by name														
			ViewBag.Customers = _unitOfWork.Customers.GetAll().OrderBy(cust => cust.FirstName).ToList();
		}



		[Route("/incidents")]
		[HttpGet]
		public async Task<IActionResult> List(string search, int? page, bool? unassigned, bool? open)
		{
			try
			{
                //set the pageSize
                var pageSize = 5;
                var pageNumber = page ?? 1; // default the pageNumber to 1 if there is no pagenumber passed in


                //get all incident
                //include Customer,Product, Technician, and DateOpened
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


		// update incident list 
		//technician select list
        [Route("update-incident/select-tech/")]
		[HttpGet]
        public IActionResult TechniciansSelectList()
        {
			try 
			{
				//get all tech
                ViewBag.Technicians = _unitOfWork.Technicians.GetAll();

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
        public IActionResult UpdateIncidentsList(int technicianID, int? page)
        {
			try
			{
                //set the pageSize
                var pageSize = 5;
                var pageNumber = page ?? 1; // default the pageNumber to 1 if there is no pagenumber passed in
                                            //IncidentViewModel vm = new();


                List<Incident> incidents = _unitOfWork.Incidents.GetAll(includeProperties: "Customer,Technician,Product")
                                            .Where(incident => incident.TechnicianID == technicianID)
                                                   .ToList();

				Technician technician = _unitOfWork.Technicians.Find(tech => tech.TechnicianID == technicianID);
				ViewBag.Technician = technician.FullName;

                if (incidents.Count() == 0)
                {
                    TempData["errorMessage"] = $"Technician {technician.FullName} Has No Open Incidents.";
                    ViewBag.Technicians = _unitOfWork.Technicians.GetAll().ToList();
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
        public IActionResult UpdateIncident(int incidentID, int technicianID)
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

		//
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
						return RedirectToAction("UpdateIncidentsList", new {technicianID = existingIncident.TechnicianID });
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
					TempData["errorMessage"] = "Incident has no ID.";
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
	
		



		[Route("/incident/add/")]

        [HttpGet]
		public IActionResult Add()
		{
			ViewBag.Action = "Add";
			IncidentsListInfo();


			return View("AddEdit", new Incident());
		}

		
		[Route("/incidents/edit/id/{id?}")]
		[HttpGet]
		public IActionResult Edit(int id)
		{
			ViewBag.Action = "Edit";


			IncidentsListInfo();

			var incident = _unitOfWork.Incidents.Find(incident => incident.IncidentID == id);


			return View("AddEdit", incident);
		}

		[HttpPost]
		public IActionResult AddEdit(Incident incident)
		{

			//check if inputs are valid
			if (ModelState.IsValid)
			{
				
				// if inputs are valid and there is incident choosen, update it
				if (incident.IncidentID != 0)
				{
					

                    var existingIncident = _unitOfWork.Incidents.Find(i=> i.IncidentID == incident.IncidentID);

					if (existingIncident != null)
					{
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
					//else to be added
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
			else
			{

				IncidentsListInfo();

				//  if inputs are not valid, do not go anywhere
				return View("AddEdit", incident);

			}
		}


		[HttpGet]
		public IActionResult Delete(int id)         // get the incident that is choosen to be deleted
		{
			var incident = _unitOfWork.Incidents.Find(incident => incident.IncidentID == id);  // find the selected incident from the DB

			return View(incident);
		}

		[HttpPost]
		public IActionResult Delete(Incident incident)
		{
			_unitOfWork.Incidents.Delete(incident); // deletes the choosen incident

			_unitOfWork.Save();    // save changes

			return RedirectToAction("List", "Incident"); // return to the List View of Incident
		}



    }
}
