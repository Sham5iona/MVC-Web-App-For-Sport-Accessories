using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Services;
using Sport_Accessories.ViewModels;

namespace Sport_Accessories.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ITwoFactorAuthentication _2FA;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly UpdateProfilePicture _updateProfilePicture;
        public User Current_User { get; set; }
        public UserController(IMapper mapper, UserManager<User> userManager,
                              ITwoFactorAuthentication _2FA,
                              SignInManager<User> signInManager,
                              ILogger<UserController> logger,
                              UpdateProfilePicture updateProfilePicture)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._2FA = _2FA;
            this._signInManager = signInManager;
            this._logger = logger;
            this._updateProfilePicture = updateProfilePicture;

        }

        public IActionResult ShowUser()
        {
            return View();
        }

        public IActionResult ChangeUserPassword()
        {
            return View();
        }

        public IActionResult ChangeUsername()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Add2FactorAuthenticationAsync(string? Email)
        {
            if (!_signInManager.IsSignedIn(User))
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
            if(await _updateProfilePicture.ChangeProfilePictureAsync(Input.FormFile, user))
            {
                _logger.LogInformation("Successfully updated the profile picture!");
                return LocalRedirect("/User/ShowUser");
            }

            ModelState.AddModelError(string.Empty, "Error! The file format is invalid!");

            return View("ShowUser", Input);
        }
    }
}
