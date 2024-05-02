using System.ComponentModel.DataAnnotations;

namespace Sport_Accessories.ViewModels
{
    public class TwoFactorAuthenticationViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
