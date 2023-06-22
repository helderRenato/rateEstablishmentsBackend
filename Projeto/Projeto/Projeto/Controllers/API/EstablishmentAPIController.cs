﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Projeto.Models;
using Projeto.Areas.Identity.Data;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;

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
                if(establishment == null)
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

            if(existingEstablishment == null)
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

    }
}