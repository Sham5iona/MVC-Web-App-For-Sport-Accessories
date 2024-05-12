using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.ViewModels;
using System.Security.Claims;

namespace Sport_Accessories.Services
{
    public interface IAdminService
    {
        public Task AddClaimsToAdminAsync(User user, AddAdminViewModel Input);

        public Task ReplaceClaimsOfAdminAsync(User user, IList<string> InputClaims);

    }
}
