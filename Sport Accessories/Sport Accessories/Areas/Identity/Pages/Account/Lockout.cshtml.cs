using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sport_Accessories.Areas.Identity.Pages.Account
{
    public class LockoutModel : PageModel
    {
        public DateTimeOffset? LockoutEnd {  get; set; }
        public IActionResult OnGet(DateTimeOffset? dateTimeOffset)
        {
            LockoutEnd = dateTimeOffset;
            return Page();
        }
    }
}
