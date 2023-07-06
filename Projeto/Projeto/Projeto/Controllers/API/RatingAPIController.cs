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
    public class RatingAPIController : Controller
    {
        private readonly ApplicationDBContext _context;

        public RatingAPIController(ApplicationDBContext context)
        {
            _context = context;
        }

       [HttpPost("rating/{id}")]
        public async Task<ActionResult> Create(int id, [FromBody] RatingTransportModel ratingAux)
        {
            //Criar o comentário e o rating
            var comment = new Comment();
            var rating = new Rating();

            comment.Text = ratingAux.Text; 
            comment.UserFK = ratingAux.UserFK;
            comment.EstablishmentFK = id;

            //Salvar o comentário
            _context.Add(comment);
            await _context.SaveChangesAsync();

            rating.Stars = ratingAux.Stars;
            rating.UserFK = ratingAux.UserFK; 
            rating.EstablishmentFK = id;

            _context.Add(rating);
            await _context.SaveChangesAsync();

            return Ok("Avaliação criada com sucesso");
        }
    }
}
