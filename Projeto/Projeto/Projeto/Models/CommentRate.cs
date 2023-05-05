using System.ComponentModel.DataAnnotations.Schema;

namespace dxz.Models
{
    public class CommentRate
    {
        [key]
        public int id {  get; set; }

        public int Likes { get; set; }

        [ForeignKey(nameof(Comment))]
        public int CommentFK { get; set; }

        [ForeignKey(nameof(User))]
        public int UserFK { get; set; }
    }
}
