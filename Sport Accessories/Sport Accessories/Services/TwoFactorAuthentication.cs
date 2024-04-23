using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Areas.Identity.Models;

namespace Sport_Accessories.Services
{
    public class TwoFactorAuthentication : ITwoFactorAuthentication
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        public TwoFactorAuthentication(UserManager<User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;  
            _emailSender = emailSender;
        }

        public async Task<bool> Enable2FA(User current_user, string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u =>
                                u.Email == email);
            if (user != null && current_user.Email == email)
            {
                var result = await _userManager.SetTwoFactorEnabledAsync(current_user, true);
                
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> SendToken(User current_user)
        {
            var token = await _userManager.GenerateTwoFactorTokenAsync(current_user,
                        TokenOptions.DefaultEmailProvider);
            string email_subject = "Two-Factor Authentication Token";
            string html_message = $"<h2>Your two-factor authentication token" +
                                    $" is {token}</h2>";

            await _emailSender.SendEmailAsync(current_user.Email, email_subject, html_message);
            return true;
        }
    }
}
