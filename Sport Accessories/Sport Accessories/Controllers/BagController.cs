using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Data;
using Sport_Accessories.Models;
using Sport_Accessories.ViewModels;

namespace Sport_Accessories.Controllers
{

    [Authorize(Roles = "User")]
    public class BagController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public BagController(ApplicationDbContext dbContext,
                             UserManager<User> userManager,
                             IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> ShowBagAsync()
        {
            IEnumerable<BagProduct> bagProducts = await _dbContext.BagProducts
                            .Include(bp => bp.Product)
                            .ThenInclude(p => p.Category)
                            .Include(bp => bp.Product)
                            .ThenInclude(p => p.Photo)
                            .Include(bp => bp.Bag)
                            .ToListAsync();

            return View(bagProducts);
        }

        public async Task<IActionResult> AddToBagAsync(Guid Id)
        {

            var product = await _dbContext.Products.FindAsync(Id);

            var bag_product = await _dbContext.BagProducts.FirstOrDefaultAsync(
                                                           bp => bp.ProductId == Id);

            //if current product isn't added to basket yet
            if (bag_product is null)
            {

                bag_product = new BagProduct();

                var user = await _userManager.GetUserAsync(User);

                var bag = _mapper.Map<Bag>(user);

                await _dbContext.Bags.AddAsync(bag);

                _mapper.Map(product, bag_product);

                _mapper.Map(bag, bag_product);

                await _dbContext.BagProducts.AddAsync(bag_product);

                await _dbContext.SaveChangesAsync();

                return RedirectToAction("ShowBag");

            }

            //add the passed product Id to temp data because when we pass
            //the error message, the view has to have a model and to get the model
            //we need to find it by this Id
            //Because its temp, after the request, the data will be erased
            TempData["ProductIdFromBag"] = Id;

            return RedirectToAction("ProductDetails", "Home", new { error_message =
                        "Error! The current product has already" +
                " been added to the basket!"});
        }

        public async Task<IActionResult> RemoveFromBag(Guid BagId)
        {
            var bag_product = await _dbContext.BagProducts
                        .FirstOrDefaultAsync(bp => bp.BagId == BagId);

            var bag = await _dbContext.Bags
                            .FirstOrDefaultAsync(b => b.BagId == BagId);

            _dbContext.BagProducts.Remove(bag_product);

            _dbContext.Bags.Remove(bag);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ShowBag");
        }

        public async Task<IActionResult> PaymentAsync()
        {
            IEnumerable<BagProduct> products_in_bag = await _dbContext.BagProducts
                                                     .Include(bp => bp.Product)
                                                     .ToListAsync();
            decimal total_price = 0;
            foreach(var bag_product in products_in_bag)
            {
                if(bag_product.Product.NewPrice is null)
                {
                    total_price += bag_product.Product.Price;
                }
                else
                {
                    total_price += bag_product.Product.NewPrice.Value;
                }
                
            }
            PaymentViewModel paymentViewModel = new PaymentViewModel();
            paymentViewModel.TotalPrice = total_price;
            return View(paymentViewModel);
        }

        public IActionResult Pay(PaymentViewModel Input)
        {
            if (ModelState.IsValid)
            {
                return View("Successfull_Payment");
            }
            
            return View("Payment", Input);
        }
    }
}
