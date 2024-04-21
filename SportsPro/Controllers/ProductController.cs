using Microsoft.AspNetCore.Mvc;
using SportsPro.DataAccess;
using SportsPro.DataAccess.Interfaces;
using SportsPro.Models;
using System.Linq;
using X.PagedList;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //constructor
        public ProductController(IUnitOfWork ctx)
        {
            _unitOfWork = ctx;
        }


        [Route("/products")]
        [HttpGet]
        public async Task<IActionResult> List(string search, int? page)
        {
            try {
                //setting the page size
                var pageSize = 5;
                //default the page number to 1
                var pageNumber = page ?? 1;

                var products = _unitOfWork.Products.GetAll().OrderBy(prod => prod.Name).ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    products = products.Where(prod => prod.Name.ToLower().Contains(search.ToLower())).ToList();
                }

                var pagedProducts = await products.ToPagedListAsync(pageNumber, pageSize);

                ViewBag.search = search;

                return View("List", pagedProducts);
            }
            catch (Exception ex) {
                TempData["errorMessage"] = $"An error occurred {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public ViewResult Add() 
        {
            ViewBag.Action = "Add";

            //return the AddEdit View With a new/empty product context
            return View("AddEdit", new Product());
        }


        [Route("/products/edit/id/{id?}")]
        [HttpGet]
        public  IActionResult Edit(int id)
        {
            try 
            {
                ViewBag.Action = "Edit";

                //find the passed in product id
                var product = _unitOfWork.Products.Find(product => product.ProductID == id);

                //check if product is found from db
                if (product == null) {
                    TempData["errorMessage"] = "Product not found.";
                    return RedirectToAction("List");
                }

                //return
                return View("AddEdit", product);
            }
            catch
            {
                TempData["errorMessage"] = "An error occurred while retrieving product details.";
                return RedirectToAction("List"); // Redirect to a suitable action, like the product list page
            }
        }

        [Route("/products/{action?}/id/{id?}")]
        [HttpPost]
        public  IActionResult AddEdit(Product product) 
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (product.ProductID == 0)
                    {
                        _unitOfWork.Products.Add(product);

                        // Add custom message when a product is added.
                        TempData["successMessage"] = $"Successfully Added  \"{product.Name}\" to Products.";
                    }
                    else
                    {
                        //find the product from the db
                        var existingProduct = _unitOfWork.Products.Find(product => product.ProductID == product.ProductID);

                        if (existingProduct != null)
                        {
                            existingProduct.Name = product.Name;
                            existingProduct.YearlyPrice = product.YearlyPrice;
                            existingProduct.ProductCode = product.ProductCode;
                            existingProduct.ReleaseDate = product.ReleaseDate;
                            existingProduct.Registrations = product.Registrations;
                        }
                        else
                        {
                            TempData["errorMessage"] = "Product to be updated not found.";
                        }

                        // Add custom message when a product is updated.
                        TempData["successMessage"] = " '" + product.Name + "'" + " Has Been updated.";
                    }

                    _unitOfWork.Save();

                    return RedirectToAction("List", "Product");
                }
                catch (Exception ex) {
                    TempData["errorMessage"] = "An error occurred: " + ex.Message;
                    return View("AddEdit", product);
                }
            }
            else 
            {
                ViewBag.Action = product.ProductID == 0 ? "Add" : "Edit";
                return View("AddEdit",product);
            }
        }

        //[Route(("/products/{action?}/id/{id?}"))]
        //[HttpGet]
        //public IActionResult Delete(int id) 
        //{
        //    try
        //    {
        //        var product = _unitOfWork.Products.Find(product => product.ProductID == id);

        //        ViewBag.asd = product.Name;

        //        return View("Delete", product);
        //    }
        //    catch (Exception ex){
        //        TempData["errorMessage"] = "An error occurred: " + ex.Message;
        //        return RedirectToAction("List", "Product");
        //    }
        //}

        [Route(("/products/{action?}/id/{id?}"))]
        [HttpPost]
        public  IActionResult Delete(int productID) 
        {
            try
            {
                var product = _unitOfWork.Products.Find(prod => prod.ProductID == productID);

                // Add custom message 
                TempData["successMessage"] = $"Product with ID: {product.ProductID} has been deleted.";


                _unitOfWork.Products.Delete(product);

                _unitOfWork.Save();

                return RedirectToAction("List", "Product");
            }
            catch (Exception ex) {
                TempData["errorMessage"] = $"Error occurred: {ex.Message}";
                return RedirectToAction("List", "Product");
            }
        }
    }
}
