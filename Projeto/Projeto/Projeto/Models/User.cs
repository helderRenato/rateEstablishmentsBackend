using System.ComponentModel.DataAnnotations.Schema;

namespace dxz.Models
{
    public class User
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [ForeignKey(nameof(User))]
        public int UserFK { get; set; }


        public ICollection<Comment> ListComment { get; set; }

        public ICollection<EstablishmentRate> ListEstablishmentRate { get; set; }

        public ICollection<CommentRate> ListCommentRate { get; set; }


        public User()
        {
            ListComment = new List<Comment>();
            ListEstablishmentRate = new List<EstablishmentRate>();
            ListCommentRate = new List<CommentRate>();
        }
    }
}
