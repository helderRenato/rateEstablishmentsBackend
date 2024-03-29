﻿using Microsoft.AspNetCore.Identity;
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
            return View(await _context.Establishment.ToListAsync()!);
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
            if (_context.Establishment == null)
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

        
        public async Task<IActionResult> PasswordReset(int? id)
        {
            if (id == null || _context.Establishment == null)
            {
                return NotFound();
            }

            var establishment = await _context.Establishment
                                  .Where(a => a.Id == id)
                                  .FirstOrDefaultAsync();


            if (establishment == null)
            {
                return NotFound();
            }

            return View(establishment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasswordReset(int id, [Bind("Id, Email, Password, Name, City, Address, Phone, TypeEstablishment")] Establishment establishment)
        {
            if (id != establishment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Transform the password to a hash password 
                    var hashPass = new PasswordHasher<Establishment>();
                    establishment.Password = hashPass.HashPassword(establishment, establishment.Password);

                    _context.Update(establishment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstablishmentExists(establishment.Id))
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

            return View(establishment);
        }

        private bool EstablishmentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Establishment == null)
            {
                return NotFound();
            }

            var establishment = await _context.Establishment
                                  .Where(a => a.Id == id)
                                  .FirstOrDefaultAsync();


            if (establishment == null)
            {
                return NotFound();
            }

            return View(establishment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Email, Password, Name, City, Address, Phone, TypeEstablishment")] Establishment establishment)
        {
            if (id != establishment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(establishment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstablishmentExists(establishment.Id))
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

            return View(establishment);
        }

        //O estabelecimento pode ter pelo menos a cinco fotografias
        public async Task<IActionResult> AddPhoto(int? id)
        {
            if (id == null || _context.Establishment == null)
            {
                return NotFound();
            }

            var establishment = await _context.Establishment
                                  .Where(a => a.Id == id)
                                  .FirstOrDefaultAsync();


            if (establishment == null)
            {
                return NotFound();
            }

            return View(establishment);
        }

        //Adicionar a fotografia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoto(int id, [Bind("Id, Email, Password, Name, City, Address, Phone, TypeEstablishment")] Establishment establishment, IFormFile foto)
        {
            if (id != establishment.Id)
            {
                return NotFound();
            }

            //Verificar se o estabelecimento já não possui o máximo de fotografias associadas (5)
            establishment = await _context.Establishment
                                  .Include(a => a.ListPhotos)
                                  .FirstOrDefaultAsync(m => m.Id == id);
            if(establishment != null)
            {
                if(establishment.ListPhotos.Count > 4)
                {
                    ModelState.AddModelError("", "Um estabelecimento pode ter no máximo 5 fotografias");
                    return View();
                }
            }

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
                try
                {
                    _context.Update(establishment);
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstablishmentExists(establishment.Id))
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

            return View(establishment);
        }

        // Eliminar a fotografia mas tendo atenção que o estabelecimento deve ter uma fotografia ao menos 
        /* public async Task<IActionResult> DeletePhoto(int id, [Bind("Id, Email, Password, Name, City, Address, Phone, TypeEstablishment")] Establishment establishment, IFormFile foto)
         {
             return View(); 
         }*/

        public async Task<IActionResult> DeletePhoto(int? id, int idEstablishment)
        {
            if (id == null || _context.Photo == null)
            {
                return NotFound();
            }

            //Verificar se o estabelecimento tem apenas uma foto
            var establishment = await _context.Establishment
                      .Include(a => a.ListPhotos)
                      .FirstOrDefaultAsync(m => m.Id == idEstablishment);
            if(establishment == null)
            {
                return NotFound();
            }
            else
            {
                if(establishment.ListPhotos.Count <= 1)
                {
                    ModelState.AddModelError("", "O estabelecimento deve ter pelo menos uma imagem");
                    return RedirectToAction(nameof(Details), new {id = idEstablishment});
                }
            }
            var photo = await _context.Photo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        [HttpPost, ActionName("DeletePhoto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePhotoConfirmed(int id)
        {
            if (_context.Photo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Photo'  is null.");
            }
            var photo = await _context.Photo.FindAsync(id);
            if (photo != null)
            {
                _context.Photo.Remove(photo);
            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            //Eliminar a imagem em disco 
            if (id != null)
            {
                // ask the server what address it wants to use
                string enderecoParaGuardarAImagem = _webHostEnvironment.WebRootPath;
                string novaLocalizacao = Path.Combine(enderecoParaGuardarAImagem, "Photos//User");
                // see if the folder 'Photos' exists
                if (!Directory.Exists(novaLocalizacao))
                {
                    Directory.CreateDirectory(novaLocalizacao);
                }
                
                novaLocalizacao = Path.Combine(novaLocalizacao, photo.Name);
                System.IO.File.Delete(novaLocalizacao);
            }
        }

    }
}
