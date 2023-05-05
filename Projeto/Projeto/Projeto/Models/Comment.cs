using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Comment
    {

        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public Boolean Denounced {get; set;}

        public Boolean IsAnswer { get; set; }

        [ForeignKey(nameof(EstablishmentFK))]
        public Establishment Establishment { get; set; }
        public int EstablishmentFK { get; set; }


        public ICollection<CommentRate> ListCommentRate { get; set; }

        public Comment()
        {
            ListCommentRate = new List<CommentRate>();
        }
    }
}
