using System.ComponentModel.DataAnnotations;

namespace Travelling_Application.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "UserName is empty")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password is not correct")]
        public string ConfirmPassword { get; set; }
        public string AccountType { get; set; }
    }
}
