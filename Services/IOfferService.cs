using Sport_Accessories.Models;

namespace Sport_Accessories.Services
{
    public interface IOfferService
    {
        public Task<Guid> GetCategoryIdAsync(string category_name);
        public Task<bool> AddOfferAsync(Product product);

        public Task<Guid> AddProductImageAsync(IFormFile? productFile, Photo photo);
    }
}
