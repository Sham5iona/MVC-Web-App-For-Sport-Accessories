using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Services;
using Sport_Accessories.ViewModels;
using AutoMapper;
using Sport_Accessories.Models;
using Sport_Accessories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Sport_Accessories.Controllers
{
    public class OfferController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<User> _userManager;
        public static string? FileName { get; set; }

        private static Guid? _product_id;

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
        [Authorize(Roles = "User")]
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
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
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

                if (Input.IsPromo && Input.NewPrice != null)
                {
                    product.NewPrice = Input.NewPrice;
                }

                Photo photo = new Photo();

                product.PhotoId = await _offerService.AddProductImageAsync(
                                                        Input.FormFile, photo);
                if (photo.PhotoId == Guid.Empty)
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

        [HttpPost]
        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteOfferAsync(Guid Id)
        {
            var product = await _dbcontext.Products.FirstOrDefaultAsync(p =>
                                                p.ProductId == Id);

            _dbcontext.Products.Remove(product);

            await _dbcontext.SaveChangesAsync();

            return RedirectToAction("ShowOffers", "Admin");
        }

        [HttpPost]
        [Authorize(Policy = "Edit")]
        public async Task<IActionResult> EditOfferAsync(Guid Id)
        {
            //get the product by the provided Id
            var product = await _dbcontext.Products.Include(p => p.Category).Include(p => p.Photo)
                                    .FirstOrDefaultAsync(p => p.ProductId == Id);

            _product_id = Id;

            //map the view model properties only to prevent accessing
            //sensitive data
            var product_view_model = _mapper.Map<OfferViewModel>(product);
            product_view_model.Categories = await _dbcontext.Categories.ToListAsync();
            FileName = product_view_model.FileName;

            //pass the view model to the view
            return View(product_view_model);
        }

        [HttpPost]
        [Authorize(Policy = "Edit")]
        public async Task<IActionResult> SubmitEditOfferAsync(OfferViewModel Input)
        {
            if (ModelState.IsValid)
            {
                var product = await _dbcontext.Products.Include(p => p.Category)
                    .Include(p => p.Photo)
                    .FirstOrDefaultAsync(p => p.ProductId == _product_id);

                _mapper.Map(Input, product);

                if (Input.IsPromo && Input.NewPrice != null)
                {
                    product.NewPrice = Input.NewPrice;
                }
                else if((!Input.IsPromo && Input.NewPrice is not null) ||
                        (Input.IsPromo && Input.NewPrice is null))
                {
                    ModelState.AddModelError(string.Empty, "To edit the new price, the" +
                        " is in promotion option must be checked! Same is otherwise!");
                    Input.Categories = await _dbcontext.Categories.ToListAsync();
                    Input.FileName = product.Photo.FileName;
                    return View("EditOffer", Input);
                }

                if (Input.CategoryName != product.Category.CategoryName)
                {
                    product.Category = _mapper.Map<Category>(product);
                    product.CategoryId = product.Category.CategoryId;
                }

                if (Input.FormFile is not null)
                {
                    bool is_photo_upload_succeded = await _offerService
                                          .ChangeProductImageAsync(Input.FormFile, product);

                    if (!is_photo_upload_succeded)
                    {

                        ModelState.AddModelError(string.Empty, "Invalid image format!");
                        Input.Categories = await _dbcontext.Categories.ToListAsync();
                        Input.FileName = product.Photo.FileName;
                        return View("EditOffer", Input);

                    }
                }

                product.User = await _userManager.FindByIdAsync(product.UserId);

                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("ShowOffers", "Admin");
            }    

            Input.Categories = await _dbcontext.Categories.ToListAsync();
            Input.FileName = FileName;
            return View("EditOffer", Input);
        }

    }
}
