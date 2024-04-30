using Sport_Accessories.Areas.Identity.Models;

namespace Sport_Accessories.Services
{
    public interface ITwoFactorAuthentication
    {
        public Task<bool> Enable2FA(User current_user, string email);

        public Task<bool> SendToken(User current_user);
    }
}
