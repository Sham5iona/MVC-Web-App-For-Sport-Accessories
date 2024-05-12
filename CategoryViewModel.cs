using System.ComponentModel.DataAnnotations;

namespace Sport_Accessories.ViewModels
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Please, enter a category name!")]
        public string? CategoryName { get; set; }

        public bool IsActive {  get; set; }

    }
}
