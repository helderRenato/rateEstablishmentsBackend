using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Rating
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public int Stars { get; set; }

        [ForeignKey(nameof(User))]
        public int UserFK { get; set; } 
        public User User { get; set; }

        [ForeignKey(nameof(Establishment))]
        public int EstablishmentFK { get; set; }
        public Establishment Establishment { get; set; }

    }
}
