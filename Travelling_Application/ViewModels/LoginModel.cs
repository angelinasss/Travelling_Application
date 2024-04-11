using System.ComponentModel.DataAnnotations;

namespace Travelling_Application.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserName is empty")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
