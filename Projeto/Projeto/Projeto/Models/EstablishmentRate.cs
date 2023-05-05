using System.ComponentModel.DataAnnotations.Schema;

namespace dxz.Models
{
    public class EstablishmentRate
    {

        [key]
        public int Id { get; set; }

        public int Stars { get; set; }

        [ForeignKey(nameof(User))]
        public int UserFK { get; set; }

        [ForeignKey(nameof(Establishment))]
        public int EstablishmentFK { get; set; }
    }
}
