using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Services;
using Sport_Accessories.ViewModels;
using System.Security.Claims;

namespace Sport_Accessories.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<SuperAdminController> _logger;
        private readonly IAdminService _adminService;
        public static User? CurrentUser { get; private set; }
        public IList<User> Users { get; set; }

        private static string _admin_id;

        public SuperAdminController(UserManager<User> userManager,
                                    SignInManager<User> signInManager,
                                    IMapper mapper,
                                    IAdminService adminService,
                                    ILogger<SuperAdminController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _adminService = adminService;
            _mapper = mapper;
            _logger = logger;
        }

        [Route("/SuperAdmins")]
        [HttpGet]
        public IActionResult SuperAdminLogin()
        {
            return View();
        }

        [Route("/SuperAdmins")]
        [HttpPost]
        public async Task<IActionResult> SuperAdminLoginAsync(
                                            AdminLoginViewModel InputViewModel)
        {
            if (ModelState.IsValid)
            {

                CurrentUser = await _userManager.FindByNameAsync(InputViewModel.UserName);

                if (CurrentUser is null)
                {
                    ModelState.AddModelError(string.Empty, "There is no user with this " +
                                            "credentials!");
                    return View();
                }

                bool isSuperAdmin = await _userManager
                                .IsInRoleAsync(CurrentUser, "SuperAdministrator");

                if (isSuperAdmin)
                {

                    var result = await _signInManager.PasswordSignInAsync(
                        InputViewModel.UserName, InputViewModel.Password, false, true);


                    if (result.Succeeded)
                    {

                        return RedirectToAction("HomeSuperAdmin");
                    }

                    ModelState.AddModelError(string.Empty, "Error! Invalid login attempt!");
                }

                ModelState.AddModelError(string.Empty, "Error! Invalid login attempt!");

            }

            return View(InputViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdministrator")]
        public async Task<IActionResult> HomeSuperAdminAsync()
        {

            if (CurrentUser is not null && await _userManager.IsInRoleAsync(CurrentUser,
                                                                "SuperAdministrator"))
            {

                Users = await _userManager.GetUsersInRoleAsync("Admin");

                return View(Users);
            }

            return StatusCode(403);
        }

        public async Task<IActionResult> ReturnToIndexAsync()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdministrator")]
        public async Task<IActionResult> AddAdminAsync()
        {
            if (User is not null && await _userManager.IsInRoleAsync(CurrentUser,
                                                                 "SuperAdministrator"))
            {
                return View();
            }

            return StatusCode(403);
        }

        [Authorize(Roles = "SuperAdministrator")]
        public async Task<IActionResult> SubmitAddAdminAsync(AddAdminViewModel Input)
        {
            if (ModelState.IsValid)
            {
                if ((Input.LockoutEnabled && (Input.LockoutEnd == null ||
                    Input.AccessFailedCount == 0) || (!Input.LockoutEnabled &&
                    (Input.LockoutEnd != null || Input.AccessFailedCount > 5))))
                {
                    ModelState.AddModelError(string.Empty, "Invalid lockout input!");
                    return View("AddAdmin", Input);

                }

                var current_username = await _userManager.FindByNameAsync(Input.UserName);

                if (current_username is null)
                {

                    var admin = _mapper.Map<User>(Input);

                    var result = await _userManager.CreateAsync(admin,
                                                    Input.PasswordHash);
                    var current_user = await _userManager.FindByNameAsync(Input.UserName);

                    var role_result = await _userManager.AddToRoleAsync(
                                                        current_user, "Admin");


                    if (result.Succeeded && role_result.Succeeded)
                    {

                        try
                        {

                            await _adminService.AddClaimsToAdminAsync(current_user,
                                                                      Input);
                        }
                        catch(Exception ex)
                        {
                            _logger.LogCritical(ex.Message);
                            return View("AddAdmin", Input);
                        }

                        return RedirectToAction("HomeSuperAdmin");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                            return View("AddAdmin", Input);
                        }
                    }
                }

                    //username is already taken
                    ModelState.AddModelError(string.Empty, "The username is already" +
                                                            " taken");
                    return View("AddAdmin", Input);

                }

            return View("AddAdmin", Input);
        }

        [Authorize(Roles = "SuperAdministrator")]
        public async Task<IActionResult> EditAdminAsync(string Id)
        {
            var admin = await _userManager.FindByIdAsync(Id);

            _admin_id = Id;

            AddAdminViewModel Input = _mapper.Map<AddAdminViewModel>(admin);

            var admin_claims = await _userManager.GetClaimsAsync(admin);

            Input.UserClaims = admin_claims;

            Input.AllClaims = new List<string> { "Create", "Read", "Delete", "Edit" };

            return View(Input);
        }

        [Authorize(Roles = "SuperAdministrator")]
        public async Task<IActionResult> SubmitEditAdminAsync(AddAdminViewModel Input,
                                                              IList<string> InputClaims)
        {

            if (ModelState.IsValid)
            {
                //check if the lockout options aren't null and the lockout enable
                // is false
                if (!Input.LockoutEnabled && (Input.LockoutEnd != null ||
                                             Input.AccessFailedCount != 0))
                {

                    ModelState.AddModelError("", "To be able to lockout, you must" +
                        " enable lockout functionality!");

                    return View("EditAdmin", Input);

                }

                //map the view models properties to the User class throught the AutoMapper
                var admin = await _userManager.FindByIdAsync(_admin_id);

                _mapper.Map(Input, admin);

                //update the current user
                var result = await _userManager.UpdateAsync(admin);

                if (result.Succeeded)
                {
                    //change his claims as well
                    try
                    {
                        await _adminService.ReplaceClaimsOfAdminAsync(admin, InputClaims);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex.Message);
                        return View("EditAdmin", Input);
                    }

                    return RedirectToAction("HomeSuperAdmin");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View("EditAdmin", Input);
                }

            }

            return View("EditAdmin", Input);
        }

        [Authorize(Roles = "SuperAdministrator")]
        public async Task<IActionResult> DeleteAdminAsync(string Id)
        {
            var admin = await _userManager.FindByIdAsync(Id);

            await _userManager.DeleteAsync(admin);

            return RedirectToAction("HomeSuperAdmin");
        }
    }
}
