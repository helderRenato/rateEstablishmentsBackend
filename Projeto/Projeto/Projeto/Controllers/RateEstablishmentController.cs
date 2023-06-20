using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rating.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["Users"] = new SelectList(_context.Users, "Id", "Username");
            ViewData["Establishments"] = new SelectList(_context.Establishment, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Stars,UserFK,EstablishmentFK")] Rating establishmentRate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(establishmentRate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
