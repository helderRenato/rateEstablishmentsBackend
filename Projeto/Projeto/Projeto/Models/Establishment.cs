using System.ComponentModel.DataAnnotations.Schema;

namespace dxz.Models
{
    public class Establishment
    {
        public int id { get; set; }

        public string name { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Mail { get; set; }

        public string phone { get; set; }

        public establishmentType TypeEstablishment { get; set; }

        public enum establishmentType {
            Restaurante,
            Café,
            Bar,
            Hotel
        }

        [ForeignKey(nameof(User))]
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
