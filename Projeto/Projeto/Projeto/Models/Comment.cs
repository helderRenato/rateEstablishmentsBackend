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
        [Required(ErrorMessage = "O comentário é de preenchimento obrigatório")]
        [StringLength(50)]
        [RegularExpression("^.{20,}$", ErrorMessage = "O comentário deve ter pelo menos 20 caractéres")]
        public string Text { get; set; }

        /// <summary>
        /// Caso o comentário for denunciado
        /// </summary>
        public Boolean Denounced {get; set;}


        public Boolean IsAnswer { get; set; }

        
        public ICollection<CommentRate> ListCommentRate { get; set; }

        public Comment()
        {
            ListCommentRate = new List<CommentRate>();
        }
    }
}
