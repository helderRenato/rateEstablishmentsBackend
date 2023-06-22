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
                var username = _context.User
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

                _context.Add(user);
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

            var users = await _context.User
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                                  .Where(a => a.Id == id)
                                  .FirstOrDefaultAsync();


            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Transform the password to a hash password 
                    var hashPass = new PasswordHasher<User>();
                    user.Password = hashPass.HashPassword(user, user.Password);

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }


    }
}
