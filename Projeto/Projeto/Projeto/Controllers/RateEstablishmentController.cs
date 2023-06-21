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
                //Obviamente o utilizador nao pode dar rating duas vezes ao mesmo restaurante
                //Caso fizer um novo rating devemos eliminar o que ele fez e adicionar o novo 

                //Verificar se o user ja tem um rating
                var ratingByUser = await _context.Rating
                                   .Where(a => a.UserFK == establishmentRate.UserFK)
                                   .FirstOrDefaultAsync();
                if(ratingByUser != null)
                {
                    //Caso tiver primeiro eliminamos o rating anterior 
                    var rating = await _context.Rating.FindAsync(ratingByUser.Id);
                    _context.Rating.Remove(rating);
                    //e depois adicionamos
                    _context.Add(establishmentRate);
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

            return RedirectToAction(nameof(Index));
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
    }
}
