using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsPro.Models
{
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegistrationID { get; set; }  // primary key

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int ProductID { get; set; }

        //navigation properties
        public Customer Customer { get; set; } 

        public Product Product { get; set; } 


    }
}
