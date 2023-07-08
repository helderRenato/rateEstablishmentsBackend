using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Projeto.Models;
using Projeto.Areas.Identity.Data;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using static Projeto.Models.Establishment;

namespace Projeto.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstablishmentAPIController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EstablishmentAPIController(ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //Login
        //Para efetuar o login o utilizador deve colocar as suas credencias nomeadamente o email e a password 
        //Caso encontre um erro nas credencias devemos retornar uma mensagem de erro
        //Caso não retornamos os dados do utilizador 
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel login)
        {
            //Verificar se o utilizador existe na base de dados

            //Verificar o email
            var establishment = await _context.Establishment.SingleOrDefaultAsync(u => u.Email == login.Email);

            if (establishment == null)
            {
                return BadRequest("Credenciais Inválidas");
            }
            else
            {
                //Verificar se a password esta correta 
                var hashPass = new PasswordHasher<Establishment>();
                var password = hashPass.VerifyHashedPassword(establishment, establishment.Password, login.Password);

                if (password == PasswordVerificationResult.Success)
                {
                    return Ok(establishment);
                }

                return BadRequest("Password Incorreta");

            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] EstablishmentTransportModel establishmentAux)
        {
            var establishment = new Establishment();

            establishment.Email = establishmentAux.Email;
            establishment.City = establishmentAux.City;
            establishment.Phone = establishmentAux.Phone;
            establishment.Name = establishmentAux.Name;
            establishment.TypeEstablishment = establishmentAux.TypeEstablishment;
            establishment.Address = establishmentAux.Address;

            var hashPass = new PasswordHasher<Establishment>();
            establishment.Password = hashPass.HashPassword(establishment, establishmentAux.Password);

            // Definir nome da imagem
            string nomeFoto = "";
            Guid g;
            g = Guid.NewGuid();
            nomeFoto = g.ToString();
            string extensao = Path.GetExtension(establishmentAux.File.FileName).ToLower();
            nomeFoto += extensao;

            //Associar a imagem ao estabelecimento na base de dados
            Photo photo = new Photo
            {
                Date = DateTime.Now,
                Name = nomeFoto
            };

            //adicionar a lista 
            establishment.ListPhotos.Add(photo);

            var email = _context.Establishment
                .Where(a => a.Email == establishment.Email)
                .FirstOrDefault();

            if (email != null)
            {
                return BadRequest("Já existe uma conta com este email");
            }

            //Salvar na base de dados os dados do estabelecimento
            _context.Add(establishment);
            await _context.SaveChangesAsync();

            if (establishmentAux.File != null)
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
                await establishmentAux.File.CopyToAsync(stream);
            }


            return Ok(establishment);
        }

        //Caso o utilizador esqueca-se da password devemos ter uma rota capaz de fazer o reset a password 
        [HttpPost("resetpass/{id}")]
        public async Task<ActionResult> ResetPass(int id, [FromBody] PasswordResetModel passwordReset)
        {
            if (ModelState.IsValid)
            {
                //Buscar o utilizador pelo Id 
                var establishment = _context.Establishment
                            .FirstOrDefault(a => a.Id == id);
                if (establishment == null)
                {
                    return BadRequest("Estabelecimento Inexistente");
                }

                //Verificar se a password e o confirmar password são iguais
                if (passwordReset.Password != passwordReset.ConfirmarPassword)
                {
                    return BadRequest("Passwords diferentes por favor insira novamente.");
                }

                try
                {
                    //Transform the password to a hash password 
                    var hashPass = new PasswordHasher<Establishment>();
                    establishment.Password = hashPass.HashPassword(establishment, passwordReset.Password);

                    _context.Update(establishment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstablishmentExists(establishment.Id))
                    {
                        return BadRequest();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(establishment);
            }

            return BadRequest();
        }

        private bool EstablishmentExists(int id)
        {
            return _context.Establishment.Any(e => e.Id == id);
        }


        //Editar os dados pertencentes ao estabelecimento 
        [HttpPost("edit/{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] EstablishmentTransportModel establishmentAux)
        {
            //Buscar o estabelecimento existente 
            var existingEstablishment = _context.Establishment.FirstOrDefault(wf => wf.Id == id);

            //Atualizar com os dados novos 
            existingEstablishment.Name = establishmentAux.Name;
            existingEstablishment.Address = establishmentAux.Address;
            existingEstablishment.Phone = establishmentAux.Phone;
            existingEstablishment.City = establishmentAux.City;
            existingEstablishment.TypeEstablishment = establishmentAux.TypeEstablishment;

            if (existingEstablishment == null)
            {
                return BadRequest("Estabelecimento Inexistente");
            }
            else
            {
                _context.Update(existingEstablishment);
                await _context.SaveChangesAsync();
                return Ok("Estabelecimento atualizado");
            }

        }


        [HttpGet("getData")]
        public async Task<ActionResult> GetData([FromQuery]int id)
        {
            //var establishmentName = _context.Establishment.FirstOrDefault(e => e.Name == name);
            //var establishmentAddress = _context.Establishment.FirstOrDefault(e => e.City == city);


            var establishment = await _context.Establishment
                    .Include(e => e.ListPhotos)
                    .Include(e => e.ListComments)
                    //.Include(e => e.ListRatings)
                    .FirstOrDefaultAsync(e => e.Id == id);

            return Ok(establishment);

        }



        //Filtrar os estabelecimentos pelo Nome e o/u o tipo de estabelecimento (Restaurante, Café, Bar, Hotel) e Cidade
        [HttpGet("getFiltered")]
        public async Task<ActionResult> GetFiltered([FromQuery] FilterEstablishmentModel establishmentAux)
        {
            //Verificar se a request veio apenas com o nome ou o tipo de estabelecimento , cidade ou os tres juntos 
            if ((establishmentAux.Name == null) && (establishmentAux.TypeEstablishment != null) && (establishmentAux.City == null))
            {
                //Significa que vamos buscar os estabelicementos apenas pelo tipo
                var establishments = await _context.Establishment
    .Include(e => e.ListPhotos)
    .Where(wf => wf.TypeEstablishment == establishmentAux.TypeEstablishment)
    .ToListAsync();
                return Ok(establishments);
            }
            else if ((establishmentAux.Name != null) && (establishmentAux.TypeEstablishment == null) && (establishmentAux.City == null))
            {
                //Significa que vamos buscar apenas pelo nome
                var establishments = await _context.Establishment
    .Include(e => e.ListPhotos)
    .Where(wf => wf.Name.Contains(establishmentAux.Name))
    .ToListAsync();
                return Ok(establishments);
            }
            else if ((establishmentAux.Name == null) && (establishmentAux.TypeEstablishment == null) && (establishmentAux.City != null))
            {
                //Significa que vamos buscar apenas pela cidade 
                var establishments = await _context.Establishment
    .Include(e => e.ListPhotos)
    .Where((wf => wf.City == establishmentAux.City))
    .ToListAsync();
                return Ok(establishments);
            }
            else if ((establishmentAux.Name != null) && (establishmentAux.TypeEstablishment == null) && (establishmentAux.City != null))
            {
                //Significa que vamos buscar pela cidade e o nome
                var establishments = await _context.Establishment
    .Include(e => e.ListPhotos)
    .Where(wf => wf.City == establishmentAux.City && wf.Name.Contains(establishmentAux.Name))
    .ToListAsync();
                return Ok(establishments);
            }
            else if ((establishmentAux.Name != null) && (establishmentAux.TypeEstablishment != null) && (establishmentAux.City == null))
            {
                //Pelo nome e o tipo 
                var establishments = await _context.Establishment
   .Include(e => e.ListPhotos)
   .Where(wf => wf.TypeEstablishment == establishmentAux.TypeEstablishment && wf.Name.Contains(establishmentAux.Name))
   .ToListAsync();
                return Ok(establishments);
            }
            else if ((establishmentAux.Name == null) && (establishmentAux.TypeEstablishment != null) && (establishmentAux.City != null))
            {
                //Pela cidade e pelo tipo
                var establishments = await _context.Establishment
    .Include(e => e.ListPhotos)
    .Where(wf => wf.TypeEstablishment == establishmentAux.TypeEstablishment && wf.City == establishmentAux.City)
    .ToListAsync();
                return Ok(establishments);
            }
            else
            {
                //Significa que vamos buscar pelos 3 parametros
                var establishments = await _context.Establishment
    .Include(e => e.ListPhotos)
    .Where(wf => wf.Name.Contains(establishmentAux.Name) && wf.TypeEstablishment == establishmentAux.TypeEstablishment && wf.City == establishmentAux.City)
    .ToListAsync(); 
                return Ok(establishments);
            }
        }

        //Adicionar fotografia ao estabelecimento
        [HttpPost("addphoto/{id}")]
        public async Task<ActionResult> AddPhoto(int id, [FromForm] IFormFile foto)
        {
            //Verificar se o estabelecimento já não possui o máximo de fotografias associadas (5)
            var establishment = await _context.Establishment
                                  .Include(a => a.ListPhotos)
                                  .FirstOrDefaultAsync(m => m.Id == id);

            if (establishment != null)
            {
                if (establishment.ListPhotos.Count > 4)
                {
                    return BadRequest("Um estabelecimento pode ter no máximo 5 fotografias");
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
                return BadRequest("Por favor selecione uma imagem");
            }

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
                    return BadRequest("Not Found");
                }
                else
                {
                    throw;
                }
            }

            return Ok("");
        }

        //Eliminar as fotografias do estabelecimento
        [HttpDelete("deletephoto/{id}")]
        public async Task<ActionResult> DeletePhoto(int id, [FromForm] int idEstablishment)
        {
            //Verificar se o estabelecimento tem apenas uma foto
            var establishment = await _context.Establishment
                      .Include(a => a.ListPhotos)
                      .FirstOrDefaultAsync(m => m.Id == idEstablishment);

            if (establishment == null)
            {
                return BadRequest("ohi");
            }
            else
            {
                if (establishment.ListPhotos.Count <= 1)
                {
                    return BadRequest("O estabelecimento deve ter pelo menos uma imagem");
                }
            }

            var photo = await _context.Photo.FindAsync(id);

            if (photo != null)
            {
                _context.Photo.Remove(photo);
            }


            await _context.SaveChangesAsync();

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

            return Ok("Fotografia eliminada com sucesso");
        }

        //Buscar as fotos que pertencem ao estabelecimento
        [HttpGet("Getphotos/{id}")]
        public async Task<ActionResult> GetPhotos(int id)
        {
            var photos =  _context.Photo
                        .Where(p => p.EstablishmentFK == id);

            return Ok(photos);
        }

    }
}
