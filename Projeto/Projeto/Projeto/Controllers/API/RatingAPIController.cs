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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RatingAPIController(ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("Avaliar")]
        public async Task<ActionResult> Avaliar([FromForm] RatingTransportModel ratingAux)
        {
            var rating = new Rating();

            rating.Stars = ratingAux.Stars;
            rating.Establishment = ratingAux.Establishment;
            rating.User = ratingAux.User;

            var user = rating.User;
            var establishment = rating.Establishment;

            user.ListRatings.Add(rating);
            establishment.ListRatings.Add(rating);


            _context.Add(rating);
            await _context.SaveChangesAsync();

            return Ok(rating);

        }
    }
}
