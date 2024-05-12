using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Data;
using Sport_Accessories.Models;
using System.Diagnostics;

namespace Sport_Accessories.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private static string? _category_name;

        public HomeController(ILogger<HomeController> logger,
                              SignInManager<User> signInManager,
                              UserManager<User> userManager,
                              ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
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
            //get all products and each relation with the products
            IEnumerable<Product> products = await _dbContext.Products.Include(p => p.Category)
                        .Include(p => p.Photo).ToListAsync();

            //pass the products to the view
            return View(products);
        }

        
        public async Task<IActionResult> SearchByProductNameAsync(string searchTerm)
        {
            IEnumerable<Product> products = await _dbContext.Products
                .Include(p => p.Category).Include(p => p.Photo).ToListAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.ToLower()
                .Contains(searchTerm.ToLower()));
            }

            return View("Index", products);
        }

        public async Task<IActionResult> SearchByCategoryNameAsync(string orderBy)
        {
            IEnumerable<Product> products = await _dbContext.Products
                .Include(p => p.Category).Include(p => p.Photo).ToListAsync();

            //order by the category name
            products = products.Where(p => p.Category.CategoryName == orderBy);

            _category_name = orderBy;

            ViewBag.Category = orderBy;

            return View("Index", products);
        }

        public async Task<IActionResult> OrderAsync(string order)
        {

            IEnumerable<Product> products = await _dbContext.Products
                .Include(p => p.Category).Include(p => p.Photo).ToListAsync();

            if(_category_name is not null)
            {
                products = _dbContext.Products
                            .Where(p => p.Category.CategoryName == _category_name);
                ViewBag.Category = _category_name;

            }

            ViewBag.Order = order;

            if (order == "Asc")
            {
                products = products.OrderBy(p => p.UpdatedAt);
            }
            else
            {
                products = products
                    .OrderByDescending(p => p.UpdatedAt);
            }

            return View("Index", products);
        }

        public async Task<IActionResult> ProductDetailsAsync(Guid? Id,
                                                             string? error_message)
        {

            if (TempData["ProductId"] is not null)
            {
                Id = Guid.Parse(TempData["ProductId"].ToString());
            }

            if(error_message is not null)
            {

                TempData["BagMessage"] = error_message;

                string product_id = TempData["ProductIdFromBag"].ToString();

                var product_to_pass = await _dbContext.Products.Include(p => p.Category)
                                .Include(p => p.Photo)
                                .Include(p => p.User)
                                .Include(p => p.ProductFavourites)
                                .FirstOrDefaultAsync(p =>
                                p.ProductId.ToString() == product_id);

                return View(product_to_pass);
            }

            var user = await _userManager.GetUserAsync(User);

            var product = await _dbContext.Products.Include(p => p.Category)
                    .Include(p => p.Photo)
                    .Include(p => p.User)
                    .Include(p => p.ProductFavourites)
                    .FirstOrDefaultAsync(p => p.ProductId == Id);

            //Here is the logic for retrieving the viewers of the offer
            //and to prevent from incrementing the viewers if the offer
            //publisher is viewing his own offer
            if (user != null && user.Id != product?.UserId)
            {
                product.Viewers += 1;

                await _dbContext.SaveChangesAsync();
            }

            if(product is null)
            {
                return StatusCode(403);
            }

            return View(product);
        }
        
    }
}
