using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Establishment
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public establishmentType TypeEstablishment { get; set; }

        public enum establishmentType {
            Restaurante,
            Café,
            Bar,
            Hotel
        }

        [ForeignKey(nameof(UserFK))]
        public User User { get; set; }
        public int UserFK { get; set; }

        public ICollection<Comment> ListComment { get; set; }

        public ICollection<EstablishmentRate> ListEstablishmentRate { get; set; }

        public ICollection<Photo> ListPhoto { get; set; }

        public Establishment()
        {
            ListComment = new List<Comment>();
            ListEstablishmentRate = new List<EstablishmentRate>();
            ListPhoto = new List<Photo>();
        }
    }
}
