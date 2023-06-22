using Microsoft.AspNetCore.Mvc;
using Projeto.Areas.Identity.Data;
using Projeto.Models;

namespace Projeto.Controllers
{

    [ApiController]
    [Route("UserTransport")]
    public class UserTransportController : ControllerBase
    {

        private ApplicationDBContext _dbcontext;

        public void UserController(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        private static ILogger _logger;


        public UserTransportController(ILogger<UserTransportController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult GetUser()
        {
            _logger.LogWarning("Entrou no método Get");
            List<User> lista = new List<User>();

            lista.ForEach((p) =>
            {
                lista.Add(new User { Username = p.Username, Password = p.Password });
            });

            _logger.LogWarning("Saiu do método Get");
            return Ok(lista);
        }


        [HttpPost("create")]
        public IActionResult CreateUser(User user)
        {
            user.Id = _dbcontext.User.Max().Id +1;
            _dbcontext.User.Add(user);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }


        [HttpPut("update")]
        public IActionResult UpdateUser(User updatedUser)
        {
            var existingUser = _dbcontext.User.FirstOrDefault(wf => wf.Id == updatedUser.Id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Username = updatedUser.Username;
            existingUser.Password = updatedUser.Password;
            existingUser.Email = updatedUser.Email;

            return NoContent();
        }


        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var existingUser = _dbcontext.User.FirstOrDefault(wf => wf.Id == id);

            if (existingUser == null)
            {
                return NotFound();
            }

            _dbcontext.User.Remove(existingUser);

            return NoContent();
        }


    }
}
