using Microsoft.AspNetCore.Identity;
using Sport_Accessories.Areas.Identity.Models;

namespace Sport_Accessories.Services
{
    public abstract class AbstractProfilePicture
    {

        public string[] Allowed_types = { "image/jpeg", "image/png",
                                            "image/jpg", "image/gif" };

        public abstract Task<bool> ChangeProfilePictureAsync(IFormFile profilePicture,
                                                             User user);
        public abstract Task<bool> AddProfilePictureAsync(
                                            IFormFile? profilePicture, User user);
    }
}
