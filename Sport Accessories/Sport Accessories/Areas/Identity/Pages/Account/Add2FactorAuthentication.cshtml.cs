using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Services;
using Sport_Accessories.ViewModels;

namespace Sport_Accessories.Areas.Identity.Pages.Account
{
    
    public class Add2FactorAuthenticationModel : PageModel
    {
        private readonly ITwoFactorAuthentication _2FA;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public static User Current_User { get; set; }

        [BindProperty]
        public TwoFactorAuthenticationViewModel Input { get; set; }

        public string Email { get; private set; }
        public Add2FactorAuthenticationModel(ITwoFactorAuthentication _2FA,
                                             UserManager<User> userManager,
                                             SignInManager<User> signInManager)
        {
            this._2FA = _2FA;
            this._userManager = userManager;
            this._signInManager = signInManager;

        }

        public async Task<IActionResult> OnGetAsync(string? Email)
        {
            if (User is null)
            {
                return StatusCode(403);

            }
            else if (!_signInManager.IsSignedIn(User))
            {
                Current_User = await _userManager.FindByEmailAsync(Email);
                
            }
            this.Email = Email;
            return Page();
        }

   
        public async Task<IActionResult> OnPostAsync(string? Email)
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
                return Page();
            }
            return Page();
        }
    }
}
