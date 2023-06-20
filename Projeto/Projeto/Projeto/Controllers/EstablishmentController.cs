using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projeto.Areas.Identity.Data;
using Projeto.Models; 

namespace Projeto.Controllers
{
    public class EstablishmentController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EstablishmentController(ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<IActionResult> Create([Bind("Id, Email, Password, Name, City, Address, Phone, TypeEstablishment")] Establishment establishment, IFormFile foto)
        {
            //Avaliar a fotografia

            //Será que o utilizador submeteu a fotografia
            string nomeFoto = ""; 
            //Caso sim vamos validar se é mesmo uma imagem
            if (foto != null)
            {
                //Caso não for .jpg,.jpeg,.png enviar mensagem de erro
                if (!(foto.ContentType == "image/jpeg" || foto.ContentType == "image/png" || foto.ContentType == "image/jpg"))
                {
                    ModelState.AddModelError("", "Por favor selecione uma imagem");
                    return View();
                }
                //Caso for uma imagem
                else
                {
                    // Definir nome da imagem
                    Guid g;
                    g = Guid.NewGuid();
                    nomeFoto = g.ToString();
                    string extensao = Path.GetExtension(foto.FileName).ToLower();
                    nomeFoto += extensao;

                    //Associar a imagem ao estabelecimento na base de dados
                    Photo photo = new Photo
                    {
                        Date = DateTime.Now,
                        Name = nomeFoto
                    };

                    //adicionar a lista 
                    establishment.ListPhotos.Add(photo);
                }
            }
            else
            {
                ModelState.AddModelError("", "Por favor selecione uma imagem");
                return View();
            }


            if (ModelState.IsValid)
            {
                //Converter a password para hash
                //Transform the password to a hash password 
                var hashPass = new PasswordHasher<Establishment>();
                establishment.Password = hashPass.HashPassword(establishment, establishment.Password);

                //Verificar se o email já não existe em outra conta
                var email = _context.Establishment
                                .Where(a => a.Email == establishment.Email)
                                .FirstOrDefault();

                if (email != null)
                {
                    ModelState.AddModelError("", "Já existe uma conta com este email");
                    return View(establishment);
                }

                //Salvar na base de dados os dados do estabelecimento
                _context.Add(establishment);
                await _context.SaveChangesAsync();

                //Guardar a imagem em disco 
                if (foto != null)
                {
                    // ask the server what address it wants to use
                    string enderecoParaGuardarAImagem = _webHostEnvironment.WebRootPath;
                    string novaLocalizacao = Path.Combine(enderecoParaGuardarAImagem, "Photos//User");
                    // see if the folder 'Photos' exists
                    if (!Directory.Exists(novaLocalizacao))
                    {
                        Directory.CreateDirectory(novaLocalizacao);
                    }
                    // save image file to disk
                    novaLocalizacao = Path.Combine(novaLocalizacao, nomeFoto);
                    using var stream = new FileStream(novaLocalizacao, FileMode.Create);
                    await foto.CopyToAsync(stream);

                    
                }

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Establishment == null)
            {
                return NotFound();
            }

            var establishment = await _context.Establishment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (establishment == null)
            {
                return NotFound();
            }

            return View(establishment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Establishment'  is null.");
            }
            var establishment = await _context.Establishment.FindAsync(id);
            if (establishment != null)
            {
                _context.Establishment.Remove(establishment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Animais/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Establishment == null)
            {
                return NotFound();
            }

            var establishment = await _context.Establishment
                                   .Include(a => a.ListPhotos)
                                   .FirstOrDefaultAsync(m => m.Id == id);

            if (establishment == null)
            {
                return NotFound();
            }

            return View(establishment);
        }
    }
}
