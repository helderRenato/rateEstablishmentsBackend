using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class CommentRate
    {
        [Key]
        public int id {  get; set; }

        public int Likes { get; set; }

        [ForeignKey(nameof(Comment))]
        public int CommentFK { get; set; }
        public Comment Comment { get; set; }

        [ForeignKey(nameof(User))]
        public int UserFK { get; set; }
        public User User { get; set; }
    }
}
