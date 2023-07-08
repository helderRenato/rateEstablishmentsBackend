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
    public class CommentAPIController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CommentAPIController(ApplicationDBContext context)
        {
            _context = context;
        }

       [HttpPost("comment")]
        public async Task<ActionResult> Create([FromBody] CommentTransportModel commentAux)
        {
            //Criar o comentário
            var comment = new Comment();

            comment.Text = commentAux.Text;
            comment.UserFK = commentAux.UserFK;
            comment.EstablishmentFK = commentAux.EstablishmentFK;
            
            _context.Add(comment);
            await _context.SaveChangesAsync();
            

            return Ok("Comentário criado com sucesso");
        }

        //Responder ao cometário
        [HttpPost("answer/{id}")]
        public async Task<ActionResult> Answer(int id, CommentTransportModel commentAux)
        {
            //Criar o comentário
            var comment = _context.Comment
                .Where(r => r.EstablishmentFK == id)
                .FirstOrDefault();


            comment.Response = commentAux.Response; 

            _context.Update(comment);
            await _context.SaveChangesAsync();


            return Ok("Comentário criado com sucesso");
        }

        //Receber os Ratings feitos pelo id do estabelecimento
        [HttpGet("getComments/{id}")]
        public async Task<ActionResult> ReceberComments(int id)
        {
            //Buscar os ratings na base de dados
            var comments = _context.Comment
                .Where(r => r.EstablishmentFK == id); 

            return Ok(comments);
        }

        //Apagar a resposta pelo id do comentário
        [HttpDelete("deleteAnswer/{id}")]
        public async Task<ActionResult> DeleteAnswer(int id)
        {
            var comment = _context.Comment
                .Where(r => r.Id == id)
                .FirstOrDefault();

            comment.Response = null;
            _context.Update(comment);
            await _context.SaveChangesAsync();

            return Ok("Resposta eliminada com sucesso");
        }

        //Apagar o comentário pelo id do comentário
        [HttpDelete("deleteComment/{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok("Comentário eliminado com sucesso");
        }

        //Denunciar o comentário 
        [HttpPost("denunciar/{id}")]
        public async Task<ActionResult> Denunciar(int id)
        {
            var comment = _context.Comment
                .Where(r => r.EstablishmentFK == id)
                .FirstOrDefault();

            comment.Denounced = true;

            _context.Update(comment);
            await _context.SaveChangesAsync();


            return Ok("Comentário denunciado com sucesso");
        }


    }
}
