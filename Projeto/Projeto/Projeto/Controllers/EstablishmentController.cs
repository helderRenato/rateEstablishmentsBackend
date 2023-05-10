using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Areas.Identity.Data;
using Projeto.Models; 

namespace Projeto.Controllers
{
    public class EstablishmentController : Controller
    {
        private readonly ApplicationDBContext _context;

        public EstablishmentController(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Establishment.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, City, Address, Phone, TypeEstablishment, User")] Establishment establishment)
        {
            //Como o username pode ser inválido quando se trata do estabelecimento então vamos atribuir um username padrão para
            //estabalecimento

            if (ModelState.IsValid)
            {
                _context.Add(establishment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
