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
            ViewData["Users"] = await _context.User.ToListAsync();
            ViewData["Establishments"] = await _context.Establishment.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, UserFK, EstablishmentFK, Stars, Comment")] EstablishmentRate establishmentRate)
        {
            ViewData["Users"] = await _context.User.ToListAsync();
            ViewData["Establishments"] = await _context.Establishment.ToListAsync();

            establishmentRate.User = _context.User
                .Where(a => a.Id == establishmentRate.UserFK)
                .FirstOrDefault();


            establishmentRate.Establishment = _context.Establishment
                .Where(a => a.Id == establishmentRate.EstablishmentFK)
                .FirstOrDefault();

            if (ModelState.IsValid)
            {
                _context.Add(establishmentRate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
