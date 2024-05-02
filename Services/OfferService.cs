using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Data;
using Sport_Accessories.Models;
using Sport_Accessories.ViewModels;

namespace Sport_Accessories.Services
{
    public class OfferService : IOfferService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<OfferService> _logger;
        public OfferService(ApplicationDbContext dbContext,
                            ILogger<OfferService> logger,
                            IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> AddOfferAsync(Product product)
        {
            try
            {
                await _dbcontext.AddAsync(product);
                await _dbcontext.SaveChangesAsync();
                return true;

            }catch (Exception ex)
            {
             _logger.LogCritical(ex.Message);
                return false;
            }

        }

        public async Task<Guid> AddProductImageAsync(IFormFile? productPicture, Photo photo)
        {
            string[] _allowed_types = { "image/jpeg", "image/png",
                                            "image/jpg", "image/gif" };

            if (!_allowed_types.Contains(productPicture.ContentType))
            {
                //invalid image format
                return Guid.Empty;
            }

            string file_name;

            if (productPicture is not null)
            {
                file_name = Guid.NewGuid().ToString() +
                                    Path.GetExtension(productPicture.FileName);

                string upload_path = Path.Combine(_webHostEnvironment.WebRootPath,
                                                                    "offers_images");

                string file_path = Path.Combine(upload_path, file_name);

                using (var file_stream = new FileStream(file_path, FileMode.Create))
                {
                    await productPicture.CopyToAsync(file_stream);
                }
            }
            else
            {
                file_name = "defaultProductImage.jpg";
            }

            photo.FileName = file_name;
            await _dbcontext.Photos.AddAsync(photo);
            await _dbcontext.SaveChangesAsync();
            Guid photo_id = (await _dbcontext.Photos.FirstOrDefaultAsync(p =>
                p.FileName == file_name)).PhotoId;

            return photo.PhotoId;
        }

        public async Task<Guid> GetCategoryIdAsync(string category_name)
        {
            var category = await _dbcontext.Categories.FirstOrDefaultAsync(c =>
                                c.CategoryName == category_name);

            if(category != null)
            {
                return category.CategoryId;
            }

            return Guid.Empty;
        }

    }
}
