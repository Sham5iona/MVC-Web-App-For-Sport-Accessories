using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Models;
using System.Diagnostics;

namespace Sport_Accessories.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger,
                              SignInManager<User> signInManager,
                              UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> IndexAsync() 
        {
            //prevent if the user is an admin or superadministrator from accessing
            //the home page because he can access the admin or superadmin panel only
            //The home page is for users or guests only!
            if (User.IsInRole("Admin") || User.IsInRole("SuperAdministrator"))
            {
                var user = await _userManager.GetUserAsync(User);
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index");

            }

            //if goes here, the User is a guest or a normal user
            return View();
        }


    }
}
