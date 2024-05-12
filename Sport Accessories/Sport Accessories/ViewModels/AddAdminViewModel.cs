using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Sport_Accessories.ViewModels
{
    public class AddAdminViewModel
    {
        [Required(ErrorMessage = "Please, enter username!")]
        public string? UserName { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public int? AccessFailedCount { get; set; }

        [Required(ErrorMessage = "Please, enter a password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at" +
                                        " max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? PasswordHash { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool CreateRole { get; set; }

        public bool ReadRole { get; set; }

        public bool EditRole { get; set; }

        public bool DeleteRole { get; set; }

        public IList<Claim>? UserClaims { get; set; }

        public IList<string>? AllClaims {  get; set; }
    }
}
