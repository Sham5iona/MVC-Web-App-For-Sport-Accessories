using System.ComponentModel.DataAnnotations;

namespace Sport_Accessories.ViewModels
{
    public class EditUserViewModel
    {
        [Required(ErrorMessage = "Please, enter username!")]
        public string? UserName { get; set; }

        public IFormFile? FormFile {  get; set; }

        public string? FileName { get; set; }

        [Required(ErrorMessage = "Please, enter an email address!")]
        [EmailAddress(ErrorMessage = "Please, enter valid email address!")]
        public string? Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public int? AccessFailedCount { get; set; }

        [Required(ErrorMessage = "Please, enter valid password")]
        [DataType(DataType.Password)]
        public string? PasswordHash { get; set; }

        public bool LockoutEnabled { get; set; }


    }
}
