using Microsoft.AspNetCore.Identity;
using Sport_Accessories.Areas.Identity.Models;

namespace Sport_Accessories.Services
{
    public class UpdateProfilePicture : AbstractProfilePicture
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UpdateProfilePicture(UserManager<User> userManager,
                                          IWebHostEnvironment webHostEnvironment)
        {
            this._userManager = userManager;
            this._webHostEnvironment = webHostEnvironment;
        }

        public override async Task<bool> ChangeProfilePictureAsync(IFormFile profilePicture,
                                                                    User user)
        {
            
                if (!_allowed_types.Contains(profilePicture.ContentType))
                {
                    //invalid image format
                    return false;
                }

                string file_name = Guid.NewGuid().ToString() +
                                  Path.GetExtension(profilePicture.FileName);

                string upload_path = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                string file_path = Path.Combine(upload_path, file_name);

                using (var file_stream = new FileStream(file_path, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(file_stream);
                }
                var oldPath = _webHostEnvironment.WebRootPath + $"/images/{user.FileName}";

                //if file already exists - delete it
                if(File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }

                user.UpdateFile(file_name);

                await _userManager.UpdateAsync(user);

                return true;
        }
    }
}

