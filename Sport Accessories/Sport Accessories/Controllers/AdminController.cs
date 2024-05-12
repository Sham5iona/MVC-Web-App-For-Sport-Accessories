using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Data;
using Sport_Accessories.Models;
using Sport_Accessories.Services;
using Sport_Accessories.ViewModels;

namespace Sport_Accessories.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AdminController> _logger;
        private readonly IMapper _mapper;
        private readonly AbstractProfilePicture _profilePicture;
        private readonly ApplicationDbContext _dbContext;
        private readonly SignInManager<User> _signInManager;
        public static User? CurrentUser {  get; private set; }
        public IList<User> Users { get; set; }

        public static string? UserId {  get; set; }

        public static string? ProfilePictureFileName {  get; set; }

        public AdminController(UserManager<User> userManager,
                               ILogger<AdminController> logger,
                               IMapper mapper,
                               AbstractProfilePicture profilePicture,
                               SignInManager<User> signInManager,
                               ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _logger = logger;
            _profilePicture = profilePicture;
            _signInManager = signInManager;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [Route("/Admins")]
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [Route("/Admins")]
        [HttpPost]
        public async Task<IActionResult> AdminLoginAsync(AdminLoginViewModel InputViewModel)
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

                bool isAdmin = await _userManager.IsInRoleAsync(CurrentUser, "Admin");

                bool isLockedOut = await _userManager.IsLockedOutAsync(CurrentUser);

                if (isAdmin && !isLockedOut)
                {
                    var result = await _signInManager.PasswordSignInAsync(InputViewModel.UserName,
                                        InputViewModel.Password, false, true);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("HomeAdmin");
                    }

                    ModelState.AddModelError(string.Empty, "Error! Invalid login attempt!");
                    return View(InputViewModel);
                }


            }
        

            ModelState.AddModelError(string.Empty, "Error! Invalid login attempt!");

            return View(InputViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "Read")]
        public async Task<IActionResult> HomeAdminAsync()
        {

            if (CurrentUser is not null && await _userManager.IsInRoleAsync(CurrentUser,
                                                                "Admin"))
            { 
            
                Users = await _userManager.GetUsersInRoleAsync("User");

                return View(Users);
            }

            return StatusCode(403);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteUserAsync(string Id)
        {
            User user = await _userManager.FindByIdAsync(Id);

            await _userManager.DeleteAsync(user);

            _logger.LogInformation("Successfully deleted a user!");

            return RedirectToAction("HomeAdmin");

        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "Edit")]
        public async Task<IActionResult> EditUserAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            UserId = Id;

            var edit_user_view_model = _mapper.Map<EditUserViewModel>(user);

            if(edit_user_view_model.LockoutEnd <= DateTimeOffset.Now)
            {
                edit_user_view_model.LockoutEnd = null;
            }

            ProfilePictureFileName = edit_user_view_model.FileName;

            return View(edit_user_view_model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "Edit")]
        public async Task<IActionResult> SubmitEditUserAsync(EditUserViewModel Input)
        {
           
            if (ModelState.IsValid)
            {

                var current_user = await _userManager.FindByIdAsync(UserId);

                _mapper.Map(Input, current_user);

                if (Input.LockoutEnd <= DateTimeOffset.UtcNow)
                {

                    Input.FileName = current_user.FileName;

                    ModelState.AddModelError("", "Lockout end date can't be" +
                                                " in the past!");
                    return View("EditUser", Input);
                }
                
                //check if the lockout options aren't null and the lockout enable
                // is false
                if(!Input.LockoutEnabled && (Input.LockoutEnd != null ||
                                             Input.AccessFailedCount != 0))
                {
                    ModelState.AddModelError("", "To be able to lockout, you must" +
                        " enable lockout functionality!");

                    Input.FileName = ProfilePictureFileName;

                    return View("EditUser", Input);

                }

                //Do the update user logic
                if (Input.FormFile is not null)
                {
                    
                    await _profilePicture.ChangeProfilePictureAsync(Input.FormFile,
                                                                    current_user);
                }

                if (Input.PasswordHash != current_user.PasswordHash)
                {
                    await _userManager.RemovePasswordAsync(current_user);

                    await _userManager.AddPasswordAsync(current_user, Input.PasswordHash);
                    
                    current_user.PasswordHash = Input.PasswordHash;
                }

                //otherwise skip to change the password because its not been changed

                var result = await _userManager.UpdateAsync(current_user);

                
                if (result.Succeeded)
                {
                    return RedirectToAction("HomeAdmin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            Input.FileName = ProfilePictureFileName;
            return View("EditUser", Input);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "Create")]
        public async Task<IActionResult> AddUserAsync()
        {
            if (User is not null && await _userManager.IsInRoleAsync(CurrentUser, "Admin"))
            {
                return View();
            }

            return StatusCode(403);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "Create")]
        public async Task<IActionResult> SubmitAddUserAsync(AddUserViewModel Input)
        {
            if (ModelState.IsValid)
            {
                if((Input.LockoutEnabled && (Input.LockoutEnd == null ||
                    Input.AccessFailedCount == 0) || (!Input.LockoutEnabled && 
                    (Input.LockoutEnd != null || Input.AccessFailedCount > 5))))
                {
                    ModelState.AddModelError(string.Empty, "Invalid lockout input!");
                    return View("AddUser", Input);
                }


                var current_email = await _userManager.FindByEmailAsync(Input.Email);
                var current_username = await _userManager.FindByNameAsync(Input.UserName);
                
                if (current_email is null)
                {

                    if (current_username is null)
                    {
                        if (Input.FormFile is not null)
                        {
                            //the logic if there is an attached file
                            //otherwise skip it because its the defaultPicture set
                            if (!_profilePicture.Allowed_types.Contains(
                                                              Input.FormFile.ContentType))
                            {
                                //check for the extension type here because to be able
                                //to add a modelstate error
                                ModelState.AddModelError(string.Empty,
                                                            "Invalid image format!");

                                return View("AddUser", Input);
                            }
                        }

                        var user = _mapper.Map<User>(Input);

                        var result = await _userManager.CreateAsync(user,
                                                        Input.PasswordHash);

                        if (result.Succeeded)
                        {

                            var current_user = await _userManager.FindByNameAsync(Input.UserName);

                            await _userManager.AddToRoleAsync(current_user, "User");

                            if (Input?.FormFile?.FileName != "defaultImage.jpg")
                            {
                                bool isSuccess = await _profilePicture.AddProfilePictureAsync(
                                                        Input?.FormFile, current_user);

                                if (isSuccess)
                                {
                                    return RedirectToAction("HomeAdmin");
                                }
                            }
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                                return View("AddUser", Input);
                            }
                        }
                    }

                    //username is already taken
                    ModelState.AddModelError(string.Empty, "The username is already" +
                                                            " taken");
                    return View("AddUser", Input);

                }

                //email is already takem
                ModelState.AddModelError(string.Empty, "The email is already taken!");
            }

            return View("AddUser", Input); 
        }
        public async Task<IActionResult> ReturnToIndexAsync()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Policy = "Read")]
        public async Task<IActionResult> ShowOffersAsync()
        {
            IList<Product> products = await _dbContext.Products.Include(p => p.Photo)
                .ToListAsync();
            return View(products);
        }

        [Authorize(Policy = "Read")]
        public async Task<IActionResult> ShowCategoriesAsync()
        {

            IList<Category> categories = await _dbContext.Categories.ToListAsync();

            return View(categories);
        }
    }
}
