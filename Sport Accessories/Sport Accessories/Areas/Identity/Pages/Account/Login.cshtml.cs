// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sport_Accessories.Areas.Identity.Models;
using AutoMapper;
using Sport_Accessories.ViewModels;

namespace Sport_Accessories.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginModel> _logger;
        public DateTimeOffset? DateTimeOffset { get; set; } = DateTime.Now;

        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger,
                            UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public LoginViewModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        
        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            if (returnUrl is not null && returnUrl.Contains("Admin"))
            {
                ModelState.AddModelError(string.Empty, "Admin or Super Admin" +
                                                       " login is forbidden!");
                return;
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            if (returnUrl is not null && returnUrl.Contains("Admin"))
            {
                ModelState.AddModelError(string.Empty, "Admin login is forbidden!");
                return Page();
            }

            returnUrl ??= Url.Content("~/");

            var user = await _userManager.FindByNameAsync(Input.Username);

            if (user is not null)
            {
                int access_failed_count = user.AccessFailedCount;

                if (ModelState.IsValid)
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true

                    bool isUser = await _userManager.IsInRoleAsync(user, "User");

                    if (isUser)
                    {

                        var result = await _signInManager.PasswordSignInAsync(
                            Input.Username, Input.Password, isPersistent: Input.RememberMe,
                            lockoutOnFailure: true);

                        if (access_failed_count > 5 || result.IsLockedOut)
                        {
                            _logger.LogWarning("User account locked out.");

                            TimeSpan defaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

                            DateTime lockoutEndLocalTime = TimeZoneInfo
                                .ConvertTimeFromUtc(
                                DateTime.UtcNow.Add(defaultLockoutTimeSpan),
                                TimeZoneInfo.Local);

                            if (user.LockoutEnd.HasValue && 
                                DateTime.Now < user.LockoutEnd.Value.DateTime)

                            {
                                DateTimeOffset = user.LockoutEnd.Value;

                                user.AccessFailedCount = 0;

                                // Reset the access failed count after lockout
                                await _userManager.UpdateAsync(user);

                                await _signInManager.SignOutAsync();

                                return RedirectToPage("./Lockout", new { DateTimeOffset });
                            }

                            await _userManager.SetLockoutEndDateAsync(user, lockoutEndLocalTime);

                            DateTimeOffset = lockoutEndLocalTime;

                            user.AccessFailedCount = 0;

                            // Reset the access failed count after lockout
                            await _userManager.UpdateAsync(user);

                            await _signInManager.SignOutAsync();

                            return RedirectToPage("./Lockout", new { DateTimeOffset });
                        }



                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in.");
                            return LocalRedirect(returnUrl);
                        }
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }

                    }
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                return Page();

            
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();

        }
    }
}
