using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projeto.Areas.Identity.Data;
using Projeto.Models;

namespace Projeto.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CommentController(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Comment.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["Users"] = new SelectList(_context.Users, "Id", "Username");
            ViewData["Establishments"] = new SelectList(_context.Establishment, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text,UserFK,EstablishmentFK")] Comment comment)
        {
            if (ModelState.IsValid)
            {

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Establishment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                                  .Where(a => a.Id == id)
                                  .FirstOrDefaultAsync();


            if (comment == null)
            {
                return NotFound();
            }

            ViewData["Users"] = new SelectList(_context.Users, "Id", "Username", comment.UserFK);
            ViewData["Establishments"] = new SelectList(_context.Establishment, "Id", "Name", comment.EstablishmentFK);
            return View(comment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,UserFK,EstablishmentFK")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["Users"] = new SelectList(_context.Users, "Id", "Username", comment.UserFK);
            ViewData["Establishments"] = new SelectList(_context.Establishment, "Id", "Name", comment.EstablishmentFK);
            return View(comment);
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                                   .Include(a => a.User)
                                   .Include(a => a.Establishment)
                                   .FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        public async Task<IActionResult> Denunciar(int? id)
        {
            if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                                  .Where(a => a.Id == id)
                                  .FirstOrDefaultAsync();


            if (comment == null)
            {
                return NotFound();
            }

            ViewData["Users"] = new SelectList(_context.Users, "Id", "Username", comment.UserFK);
            ViewData["Establishments"] = new SelectList(_context.Establishment, "Id", "Name", comment.EstablishmentFK);
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Denunciar(int id, [Bind("Id,Text,UserFK,EstablishmentFK")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    comment.Denounced = true; 
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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

            ViewData["Users"] = new SelectList(_context.Users, "Id", "Username", comment.UserFK);
            ViewData["Establishments"] = new SelectList(_context.Establishment, "Id", "Name", comment.EstablishmentFK);

            return View(comment);
        }
    }
}
