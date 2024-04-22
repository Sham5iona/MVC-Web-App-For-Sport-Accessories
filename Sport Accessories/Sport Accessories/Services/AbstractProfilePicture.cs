using Microsoft.AspNetCore.Identity;
using Sport_Accessories.Areas.Identity.Models;

namespace Sport_Accessories.Services
{
    public abstract class AbstractProfilePicture
    {

        protected string[] _allowed_types = { "image/jpeg", "image/png",
                                            "image/jpg", "image/gif" };

        public abstract Task<bool> ChangeProfilePictureAsync(IFormFile profilePicture,
                                                             User user);
        

    }
}
