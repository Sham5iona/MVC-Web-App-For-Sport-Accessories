using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Services;
using Sport_Accessories.ViewModels;
using AutoMapper;
using Sport_Accessories.Models;
using Sport_Accessories.Data;
using Microsoft.EntityFrameworkCore;

namespace Sport_Accessories.Controllers
{
    public class OfferController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<User> _userManager;
        public OfferController(SignInManager<User> signInManager,
                               IOfferService offerService,
                               UserManager<User> userManager,
                               IMapper mapper,
                               ApplicationDbContext dbContext)
        {
            _signInManager = signInManager;
            _offerService = offerService;
            _mapper = mapper;
            _dbcontext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> AddOfferAsync()
        {

            bool IsSignedIn = _signInManager.IsSignedIn(User);

            //if the user is not logged in -> redirect to the login page and after that
            //he can create an offer
            if (IsSignedIn)
            {
                var viewModel = new OfferViewModel();
                viewModel.Categories = await _dbcontext.Categories.ToListAsync();
                return View(viewModel);

            }

            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity"});
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOfferAsync(OfferViewModel Input)
        {
            //Check if all input data is valid
            if (ModelState.IsValid)
            {

                Guid category_id = await _offerService.GetCategoryIdAsync(
                                                        Input.CategoryName);

                var user = await _userManager.GetUserAsync(User);
                var product = _mapper.Map<Product>(Input);
                product.CategoryId = category_id;
                product.UserId = user.Id;
                if(Input.IsPromo && Input.NewPrice != null)
                {
                    product.NewPrice = Input.NewPrice;
                }
                Photo photo = new Photo();
                
                product.PhotoId = await _offerService.AddProductImageAsync(
                                                        Input.FormFile, photo);
                if(photo.PhotoId == Guid.Empty)
                {

                    ModelState.AddModelError(string.Empty, "Invalid image format!");

                    var viewModel = new OfferViewModel();

                    viewModel.Categories = await _dbcontext.Categories.ToListAsync();

                    return View(viewModel);
                }

                bool is_succeded = await _offerService.AddOfferAsync(product);

                if (is_succeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(Input);
            }
            Input.Categories = await _dbcontext.Categories.ToListAsync();
            return View(Input);
        }
    }
}
