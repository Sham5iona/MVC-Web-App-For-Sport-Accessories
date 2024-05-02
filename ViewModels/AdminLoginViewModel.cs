using System.ComponentModel.DataAnnotations;

namespace Sport_Accessories.ViewModels
{
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "Please, enter admin username!")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Please, enter admin password!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please, confirm admin password!")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
