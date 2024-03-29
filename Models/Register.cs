using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace InventoryWorld.Models
{
    public class Register
    {
        [BindRequired]
        public string FirstName { get; set; }

        [BindRequired]
        public string LastName { get; set; }
        [BindRequired]
        [EmailAddress]
        public string Email { get; set; }

        [BindRequired]
        public string Role { get; set; }
        [BindRequired]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [BindRequired]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
