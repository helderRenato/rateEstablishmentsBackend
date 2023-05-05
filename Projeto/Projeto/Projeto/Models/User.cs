using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        

        public ICollection<EstablishmentRate> ListEstablishmentRate { get; set; }

        public ICollection<CommentRate> ListCommentRate { get; set; }


        public User()
        {

            ListEstablishmentRate = new List<EstablishmentRate>();
            ListCommentRate = new List<CommentRate>();
        }
    }
}
