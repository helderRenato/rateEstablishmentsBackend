using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace Projeto.Models
{
    public class CommentRate
    {
        public string Id { get; set; }

        public int Likes { get; set; }


        [ForeignKey(nameof(Comentario))]
        public int CommentFK { get; set; }
    }
}
