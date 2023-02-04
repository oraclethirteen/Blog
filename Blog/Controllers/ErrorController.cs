using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Oops()
        {
            return View();
        }
    }
}
