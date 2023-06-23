using Microsoft.AspNetCore.Mvc;

namespace Projeto.Controllers.API
{
    public class RatingAPIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
