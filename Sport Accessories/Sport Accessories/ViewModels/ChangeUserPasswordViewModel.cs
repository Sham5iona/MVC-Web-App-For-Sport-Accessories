using System.ComponentModel.DataAnnotations;

namespace Sport_Accessories.ViewModels
{
    public class ChangeUserPasswordViewModel
    {
        [Required(ErrorMessage = "Please, enter your current password!")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Please, enter your new password!")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Please, confirm your new password!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmNewPassword {  get; set; }
    }
}
