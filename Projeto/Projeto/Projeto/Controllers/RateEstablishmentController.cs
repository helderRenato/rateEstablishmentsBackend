using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Areas.Identity.Data;
using Projeto.Models;

namespace Projeto.Controllers
{
    public class RateEstablishmentController : Controller
    {

        private readonly ApplicationDBContext _context;

        public RateEstablishmentController(ApplicationDBContext context)
        {

            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context.EstablishmentsRate.ToListAsync());
        }

        public async Task<IActionResult> CreateAsync()
        {
            ViewData["Users"] = await _context.Users.ToListAsync();
            ViewData["Establishments"] = await _context.Establishment.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, User, Establishment, Stars, Comment")] EstablishmentRate establishmentRate)
        {
            if (ModelState.IsValid)
            {
                 
            }

            return View();
        }
    }
}
