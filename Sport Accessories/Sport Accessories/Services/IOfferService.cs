using Sport_Accessories.Models;
using Sport_Accessories.ViewModels;

namespace Sport_Accessories.Services
{
    public interface IOfferService
    {
        public Task<Guid> GetCategoryIdAsync(string category_name);
        public Task<bool> AddOfferAsync(Product product);

        public Task<Guid> AddProductImageAsync(IFormFile? productFile, Photo photo);

        public Task<bool> ChangeProductImageAsync(IFormFile? productFile, Product product);

    }
}
