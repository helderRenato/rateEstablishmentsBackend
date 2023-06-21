using System.ComponentModel;
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

        [StringLength(50)]
        [RegularExpression("^.{20,}$", ErrorMessage = "O comentário deve ter pelo menos 20 caractéres")]
        public string? Response { get; set; }

        /// <summary>
        /// Caso o comentário for denunciado
        /// </summary>
        [DefaultValue(false)]
        public Boolean Denounced {get; set;}

        [ForeignKey(nameof(User))]
        public int UserFK { get; set; }
        public User? User { get; set; }

        [ForeignKey(nameof(Establishment))]
        public int EstablishmentFK { get; set; }
        public Establishment? Establishment { get; set; }
    }
}
