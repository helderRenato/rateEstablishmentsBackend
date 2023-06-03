using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Areas.Identity.Data;
using Projeto.Models; 

namespace Projeto.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDBContext _context;

        public UserController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Email,Username, Password")] User user)
        {
            if (ModelState.IsValid)
            {
                //Transform the password to a hash password 
                var hashPass = new PasswordHasher<User>();
                user.Password = hashPass.HashPassword(user, user.Password);

                //Verificar se o username do utilizador é único
                var username = _context.Users
                                .Where(a => a.Username == user.Username)
                                .FirstOrDefault();

                if (username != null)
                {
                    ModelState.AddModelError("", "O username já existe, por favor introduza um diferente.");
                    return View(user);
                }

                //Verificar se o email é único
                var email = _context.Users
                                .Where(a => a.Email == user.Email)
                                .FirstOrDefault();

                if (email != null)
                {
                    ModelState.AddModelError("", "Já existe uma conta com este email");
                    return View(user);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var criadores = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (criadores == null)
            {
                return NotFound();
            }

            return View(criadores);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var criadores = await _context.Users.FindAsync(id);
            if (criadores != null)
            {
                _context.Users.Remove(criadores);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
