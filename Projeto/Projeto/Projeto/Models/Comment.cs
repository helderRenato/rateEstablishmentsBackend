using System.ComponentModel.DataAnnotations.Schema;

namespace dxz.Models
{
    public class Comment
    {

        [key]
        public int Id { get; set; }

        public string Text { get; set; }

        public Boolean Denounced {get; set;}

        public Boolean IsAnswer { get; set; }

        [ForeignKey(nameof(Establishment))]
        public int EstablishmentFK { get; set; }


        public ICollection<CommentRate> ListCommentRate { get; set; }

        public Comment()
        {
            ListCommentRate = new List<CommentRate>();
        }
    }
}
