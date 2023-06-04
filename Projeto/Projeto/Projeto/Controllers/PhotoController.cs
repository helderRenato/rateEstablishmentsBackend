using Microsoft.AspNetCore.Mvc;

namespace Projeto.Controllers
{
    public class PhotoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
