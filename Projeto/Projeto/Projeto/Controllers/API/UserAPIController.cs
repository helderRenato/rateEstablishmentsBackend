using Microsoft.AspNetCore.Mvc;
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
    public class UserAPIController : Controller
    {
        private readonly ApplicationDBContext _context;

        public UserAPIController(ApplicationDBContext context)
        {
            _context = context;
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
            var utilizador = await _context.User.SingleOrDefaultAsync(u => u.Email == login.Email);
            if (utilizador == null)
            {
                return BadRequest("Credenciais Inválidas");
            }
            else
            {
                //Verificar se a password esta correta 
                var hashPass = new PasswordHasher<User>();
                var password = hashPass.VerifyHashedPassword(utilizador, utilizador.Password, login.Password);

                if(password == PasswordVerificationResult.Success)
                {
                    return Ok(utilizador);
                }

                return BadRequest("Password Incorreta");

            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserTransportModel userAux)
        {
            var user = new User(); 
            user.Email = userAux.Email;
            user.Password = userAux.Password;
            user.Username = userAux.Username;

            //Transform the password to a hash password 
            var hashPass = new PasswordHasher<User>();
            user.Password = hashPass.HashPassword(user, user.Password);

            //Verificar se o username do utilizador é único
            var username = _context.User
                                .Where(a => a.Username == user.Username)
                                .FirstOrDefault();

           if (username != null)
           {
               return BadRequest("O username já existe, por favor introduza um diferente.");
           }

                
           
           //Verificar se o email é único
            var email = _context.User
                                .Where(a => a.Email == user.Email)
                                .FirstOrDefault();

           if (email != null)
           {
                return BadRequest("Já existe uma conta com este email");
           }

           _context.Add(user);
           await _context.SaveChangesAsync();
           return Ok(user);
        }

        //Caso o utilizador esqueca-se da password devemos ter uma rota capaz de fazer o reset a password 
        [HttpPost("resetpass/{id}")]
        public async Task<ActionResult> ResetPass(int id, [FromBody] PasswordResetModel passwordReset)
        {
            //Buscar o utilizador pelo Id 
            var user = _context.User
                        .FirstOrDefault(a => a.Id == id);

            if(user == null)
            {
                return BadRequest("Utilizador Inexistente");
            }
            //Verificar se a password e o confirmar password são iguais
            if (passwordReset.Password != passwordReset.ConfirmarPassword)
            {
                return BadRequest("Passwords diferentes por favor insira novamente."); 
            }

            try
            {
                //Transform the password to a hash password 
                var hashPass = new PasswordHasher<User>();
                user.Password = hashPass.HashPassword(user, passwordReset.Password);

                _context.Update(user);
                await _context.SaveChangesAsync();
                }
            catch (DbUpdateConcurrencyException)
            {
            
                if (!UserExists(user.Id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
