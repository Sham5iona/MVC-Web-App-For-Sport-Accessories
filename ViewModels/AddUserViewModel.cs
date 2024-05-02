using System.ComponentModel.DataAnnotations;

namespace Sport_Accessories.ViewModels
{
    public class AddUserViewModel
    {
        [Required(ErrorMessage = "Please, enter username!")]
        public string? UserName { get; set; }

        public IFormFile? FormFile { get; set; }

        public string? FileName { get; set; }

        [Required(ErrorMessage = "Please, enter an email address!")]
        [EmailAddress(ErrorMessage = "Please, enter valid email address!")]
        public string? Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public int? AccessFailedCount { get; set; }

        [Required(ErrorMessage = "Please, enter a password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at" +
                                        " max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? PasswordHash { get; set; }

        public bool LockoutEnabled { get; set; }

    }
}
