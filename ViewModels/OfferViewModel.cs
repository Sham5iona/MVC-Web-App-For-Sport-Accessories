using Sport_Accessories.Models;
using System.ComponentModel.DataAnnotations;

namespace Sport_Accessories.ViewModels
{
    public class OfferViewModel
    {
        [Required(ErrorMessage = "Please, enter a product name")]
        [Display(Name = "product name")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "There are no active categories!")]
        [Display(Name = "category name")]
        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "Please, enter product description!")]
        [Display(Name = "product description")]
        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "Please, provide a currency of the price!")]
        [Display(Name = "price currency")]
        public char? Currency { get; set; }

        public bool IsPromo {  get; set; }

        [Required(ErrorMessage = "Please, enter a product price!")]
        [Range(1.0, Double.MaxValue, ErrorMessage = "Invalid price range!")]
        [Display(Name = "product price")]
        public decimal? Price { get; set; }

        [Display(Name = "new price")]
        public decimal? NewPrice { get; set; }

        public IFormFile? FormFile { get; set; }

        public string? FileName { get; set; }

        //property to store the categories to be able to choose a category name
        public IEnumerable<Category>? Categories { get; set; }
    }
}
