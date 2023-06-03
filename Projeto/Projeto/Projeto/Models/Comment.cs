using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Comment
    {

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// O texto no comentário
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Caso o comentário for denunciado
        /// </summary>
        public Boolean Denounced {get; set;}


        public Boolean IsAnswer { get; set; }

        [ForeignKey(nameof(Establishment))]
        public int EstablishmentFK { get; set; }
        public Establishment Establishment { get; set; }
        


        public ICollection<CommentRate> ListCommentRate { get; set; }

        public Comment()
        {
            ListCommentRate = new List<CommentRate>();
        }
    }
}
