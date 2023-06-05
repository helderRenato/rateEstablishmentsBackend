using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class EstablishmentRate
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public int Stars { get; set; }

        [ForeignKey(nameof(User))]
        [Required(ErrorMessage = "O username é de preenchimento obrigatório")]
        public int UserFK { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Establishment))]
        [Required(ErrorMessage = "O estabelecimento é de preenchimento obrigatório")]
        public int EstablishmentFK { get; set; }
        public Establishment Establishment { get; set; }

        /// <summary>
        /// O comentário que faz parte da avaliação
        /// </summary>
        [ForeignKey(nameof(Comment))]
        [Required(ErrorMessage = "O comentário é de preenchimento obrigatório")]
        public int CommentFK { get; set; }
        public Comment Comment { get; set; }

    }
}
