using SportsPro.DataAccess;
using SportsPro.DataAccess.Interfaces;

namespace SportsPro.Models.Validations
{
    public static class Validate
    {

        public static string CheckIfCustomerEmailExists(IUnitOfWork context, string email)
        {
            string message  = string.Empty;

            if (!string.IsNullOrEmpty(email)) 
            {
                //checks if anyone has the same email 
                var customer = context.Customers.Find(x => x.Email.ToLower() == email.ToLower());

                //if var customer is not emtpy, let the user know that the email is used.
                // customers can't have the same email
                if (customer != null) 
                {
                    message = "Email Address Is Already In Use.";
                }

            }
            return message;
        }

        public static string CheckIfTechnicianEmailExists(IUnitOfWork context, string email)
        {
            string message = string.Empty;

            if (!string.IsNullOrEmpty(email))
            {
                //checks if anyone has the same email 
                var customer = context.SportsProUsers.Find(x => x.Email.ToLower() == email.ToLower());

                //if var customer is not emtpy, let the user know that the email is used.
                // customers can't have the same email
                if (customer != null)
                {
                    message = "Email Address Is Already In Use.";
                }

            }
            return message;
        }

     
    }
}
