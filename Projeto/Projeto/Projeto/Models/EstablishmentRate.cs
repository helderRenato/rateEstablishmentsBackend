using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class EstablishmentRate
    {

        [Key]
        public int Id { get; set; }

        public int Stars { get; set; }

        [ForeignKey(nameof(User))]
        public int UserFK { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Establishment))]
        public int EstablishmentFK { get; set; }
        public Establishment Establishment { get; set; }
        
    }
}
