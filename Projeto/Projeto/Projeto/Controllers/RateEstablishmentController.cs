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
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context.Rating.ToListAsync());
        }

        public async Task<IActionResult> CreateAsync()
        {
            ViewData["Users"] = new SelectList(_context.Users, "Id", "Username");
            ViewData["Establishments"] = new SelectList(_context.Establishment, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, UserFK, EstablishmentFK, Stars, Comment")] Rating establishmentRate)
        {
            ViewData["Users"] = new SelectList(_context.Users, "Id", "Username");
            ViewData["Establishments"] = new SelectList(_context.Establishment, "Id", "Name");

            var rating = _context.Rating
                .Where(r => r.UserFK == establishmentRate.UserFK && r.EstablishmentFK == rating.EstablishmentFK)
                .FirstOrDefault(); 
                
            if (ModelState.IsValid)
            {
                //Caso o utilizador possua algum rating podemos simplesmente atualizar

                if (rating != null)
                {
                    rating.Stars = establishmentRate.Stars; 
                    _context.Update(rating);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    _context.Add(establishmentRate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Rating == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                                   .Include(a => a.User)
                                   .Include(a => a.Establishment)
                                   .FirstOrDefaultAsync(m => m.Id == id);

            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rating == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rating == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Rating'  is null.");
            }
            var rating = await _context.Rating.FindAsync(id);

            if (rating != null)
            {
                _context.Rating.Remove(rating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}