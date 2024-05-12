﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Data;
using Sport_Accessories.Models;

namespace Sport_Accessories.Controllers
{
    [Authorize(Roles = "User")]
    public class FavouriteController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public FavouriteController(ApplicationDbContext dbContext,
                                   UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToFavouritesAsync(Guid Id)
        {
            var user = await _userManager.GetUserAsync(User);

            var product = await _dbContext.Products.FindAsync(Id);

            var current_favourite = await _dbContext.Favourites
                            .FirstOrDefaultAsync(pf => pf.UserId == user.Id);

            // Store the product ID in TempData
            TempData["ProductId"] = product.ProductId;

            if (current_favourite == null)
            {
                Favourite favourite = new Favourite(user.Id);

                await _dbContext.Favourites.AddAsync(favourite);

                await _dbContext.SaveChangesAsync();

                ProductFavourite productFavourite = new ProductFavourite(product.ProductId,
                                                             favourite.FavouriteId);

                await _dbContext.ProductFavourite.AddAsync(productFavourite);

                await _dbContext.SaveChangesAsync();

                return RedirectToAction("ProductDetails", "Home");
            }

            //the product is already added to favourites and here is the logic to
            //remove it

            var product_favourite = await _dbContext.ProductFavourite
                        .FirstOrDefaultAsync(pf => pf.ProductId == Id &&
                        pf.FavouriteId == current_favourite.FavouriteId);

            _dbContext.ProductFavourite.Remove(product_favourite);

            _dbContext.Favourites.Remove(current_favourite);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ProductDetails", "Home");
        }

    }
}
