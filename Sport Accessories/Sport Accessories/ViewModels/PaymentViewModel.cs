using Sport_Accessories.Models;
using System.ComponentModel.DataAnnotations;

namespace Sport_Accessories.ViewModels
{
    public class PaymentViewModel
    {
        [Required(ErrorMessage = "Please, enter an address!")]
        [Display(Name = "address")]
        public string? Address { get; set; }

        [Display(Name = "phone number")]
        [Required(ErrorMessage = "Please, enter a phone number!")]
        [Phone(ErrorMessage = "Phone number must be numbers!")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string? PhoneNumber { get; set; }

        public decimal TotalPrice { get; set; }

        public int? Count { get; set; }

    }
}
