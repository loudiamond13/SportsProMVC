using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsPro.Models
{
    public class Incident
    {
        public int IncidentID { get; set; }


        [ForeignKey("Customer")] // name of the navigation property
        [Required]
        public int CustomerID { get; set; }     // foreign key property
        [ValidateNever]
        public Customer? Customer { get; set; }  // navigation property


        [ForeignKey("Product")] // name of the navigation property
        [Required(ErrorMessage = "Product is required")]
        public int ProductID { get; set; }     // foreign key property
        [ValidateNever]
        public Product? Product { get; set; }   // navigation property


        [ForeignKey("Technician")] // name of the navigation property
        public string? TechnicianID { get; set; } // foreign key property
        public SportsProUser? Technician { get; set; }   // navigation property



        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public DateTime DateOpened { get; set; } = DateTime.Now;

        public DateTime? DateClosed { get; set; }
    }
}