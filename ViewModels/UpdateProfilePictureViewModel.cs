using System.ComponentModel.DataAnnotations;

namespace Sport_Accessories.ViewModels
{
    public class UpdateProfilePictureViewModel
    {
        public IFormFile? FormFile { get; set; }
        public string? ProfilePictureUrl { get; set; }

        [Required]
        public string? Username { get; set; }

    }
}
