using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Sport_Accessories.Areas.Identity.Models;

namespace Sport_Accessories.Areas.Identity.Pages.Account
{
    public class LockoutModel : PageModel
    {
        public DateTimeOffset? LockoutEnd {  get; set; }
        public async Task<IActionResult> OnGetAsync(DateTimeOffset? dateTimeOffset)
        {
            LockoutEnd = dateTimeOffset;
            return Page();
        }
    }
}
