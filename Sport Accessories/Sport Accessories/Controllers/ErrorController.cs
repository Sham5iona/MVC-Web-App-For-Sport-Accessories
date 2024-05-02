using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Sport_Accessories.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Errors/{StatusCode}")]
        public IActionResult ErrorHandling(int StatusCode)
        {
            ViewBag.Status = StatusCode;
            ViewBag.Message = ReasonPhrases.GetReasonPhrase(StatusCode);
            return View();
        }
    }
}
