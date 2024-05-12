using Microsoft.AspNetCore.Identity;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.ViewModels;
using System.Security.Claims;

namespace Sport_Accessories.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        public AdminService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task AddClaimsToAdminAsync(User user, AddAdminViewModel Input)
        {

            //if any role is true from the ViewModel, add a claim to the current user 
            try
            {
                if (Input.CreateRole)
                {
                    Claim create_claim = new Claim("Ability", "Create");

                    await _userManager.AddClaimAsync(user, create_claim);
                }

                if (Input.EditRole)
                {
                    Claim edit_claim = new Claim("Ability", "Edit");

                    await _userManager.AddClaimAsync(user, edit_claim);
                }

                if (Input.DeleteRole)
                {
                    Claim delete_claim = new Claim("Ability", "Delete");

                    await _userManager.AddClaimAsync(user, delete_claim);
                }

                if (Input.ReadRole)
                {
                    Claim read_claim = new Claim("Ability", "Read");

                    await _userManager.AddClaimAsync(user, read_claim);
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task ReplaceClaimsOfAdminAsync(User user,
                                                    IList<string> InputClaims)
        {
            try
            {
                // Replace all existing claims of the user
                var claims = await _userManager.GetClaimsAsync(user);

                foreach (var claim in claims)
                {
                    if (!InputClaims.Contains(claim.ToString()))
                    {
                        await _userManager.RemoveClaimAsync(user, claim);
                    }
                }

                foreach(var claim in InputClaims)
                {
                    var newClaim = new Claim("Ability", claim);
                    if (!claims.Contains(newClaim))
                    {
                        await _userManager.AddClaimAsync(user, newClaim);
                    }
                }

            }


            catch (Exception ex)
            {
                // Handle the exception appropriately (log, return error response, etc.)
                throw new Exception(ex.Message);
            }
        }

    }
}
