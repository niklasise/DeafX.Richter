using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeafX.Richter.Web.Models.Account
{
    public class LoginViewModel : IValidatableObject
    {

        [Required(ErrorMessage = "Du måste fylla i användarnamn")]
        public string username { get; set; }

        [Required(ErrorMessage = "Du måste fylla i lösenord")]
        public string password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if(username == password)
            {
                result.Add(new ValidationResult("Användarnamn och lösenord får ej vara lika"));
            }

            return result;
        }
    }
}
