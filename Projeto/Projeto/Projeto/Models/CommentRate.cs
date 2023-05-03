using System.ComponentModel.DataAnnotations.Schema;

namespace dxz.Models
{
    public class CommentRate
    {

        public int id {  get; set; }

        public int Likes { get; set; }

        [ForeignKey(nameof(Comment))]
        public int CommentFK { get; set; }
    }
}
