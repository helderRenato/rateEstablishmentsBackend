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
            //Criar o rating
            var rating = new Rating();

            rating.Stars = ratingAux.Stars;
            rating.UserFK = ratingAux.UserFK; 
            rating.EstablishmentFK = id;

            var rating2 = _context.Rating
                    .Where(r => r.UserFK == rating.UserFK && r.EstablishmentFK == rating.EstablishmentFK)
                    .FirstOrDefault();

            //Caso o utilizador possua algum rating podemos simplesmente atualizar
            if (rating2 != null)
            {
                rating2.Stars = rating.Stars;
                _context.Update(rating2);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
            }

            return Ok("Avaliação criada com sucesso");
        }

        //Receber os Ratings feitos
        [HttpGet("getRatings/{id}")]
        public async Task<ActionResult> ReceberRatings(int id)
        {
            //Buscar os ratings na base de dados
            var ratings = _context.Rating
                .Where(r => r.EstablishmentFK == id); 

            return Ok(ratings);
        }

    }
}
