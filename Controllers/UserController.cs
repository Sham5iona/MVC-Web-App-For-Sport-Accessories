using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Services;
using Sport_Accessories.ViewModels;

namespace Sport_Accessories.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ITwoFactorAuthentication _2FA;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly AbstractProfilePicture _abstractProfilePicture;
        public UpdateProfilePictureViewModel UpdateProfilePictureViewModel { get; set; }
        public User Current_User { get; set; }
        public UserController(UserManager<User> userManager,
                              ITwoFactorAuthentication _2FA,
                              SignInManager<User> signInManager,
                              ILogger<UserController> logger,
                              AbstractProfilePicture abstractProfilePicture)
        {
            this._userManager = userManager;
            this._2FA = _2FA;
            this._signInManager = signInManager;
            this._logger = logger;
            this._abstractProfilePicture = abstractProfilePicture;

        }

        [HttpGet]
        public IActionResult ShowUser()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return View();
            }
            return StatusCode(403);
        }

        [HttpGet]
        public IActionResult ChangeUserPassword()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return View();
            }
            return StatusCode(403);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPasswordAsync(
                                        ChangeUserPasswordViewModel Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(this.User);
                if (await _userManager.CheckPasswordAsync(user, Input.OldPassword))
                {
                    var result = await _userManager.ChangePasswordAsync(user,
                                            Input.OldPassword, Input.NewPassword);
                    if (result.Succeeded)
                    {
                        _logger.LogCritical("Successfully changed the password!");
                        return View("ShowUser");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return View();
                    }

                }

                ModelState.AddModelError(string.Empty, "Error! Invalid current password!");
            }

            return View(Input);

        }

        [HttpPost]
        public async Task<IActionResult> ChangeUsernameAsync(string username)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(this.User);
                var result = await _userManager.SetUserNameAsync(user, username);

                if (result.Succeeded)
                {
                    return RedirectToAction("ShowUser");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                }
            }

            return View("ShowUser", username);


        }

        [HttpGet]
        public async Task<IActionResult> Add2FactorAuthenticationAsync(string? Email)
        {
            if (!_signInManager.IsSignedIn(User) && Email is null)
            {
                return StatusCode(403);
                
            }else if (!_signInManager.IsSignedIn(User))
            {
                Current_User = await _userManager.FindByEmailAsync(Email);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add2FactorAuthenticationAsync(
                                         TwoFactorAuthenticationViewModel Input)
        {
            if (ModelState.IsValid)
            {
                Current_User = await _userManager.FindByEmailAsync(Input.Email);

                if (await _2FA.Enable2FA(Current_User, Input.Email))
                {
                    await _2FA.SendToken(Current_User);
                    await _signInManager.SignOutAsync();
                    await _userManager.UpdateAsync(Current_User);

                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                ModelState.AddModelError(string.Empty, "The email doesn't belong to the" +
                    " current user!");
                return View();
            }
            return View(Input);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePictureAsync(
                                         UpdateProfilePictureViewModel Input)
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (await _abstractProfilePicture.ChangeProfilePictureAsync(Input.FormFile, user))
            {
                _logger.LogInformation("Successfully updated the profile picture!");
                return LocalRedirect("/User/ShowUser");
            }

            ModelState.Remove(nameof(Input.Username));

            ModelState.AddModelError(string.Empty, "Error! The file format is invalid!");

            return View("ShowUser", Input);
        }

    }
}