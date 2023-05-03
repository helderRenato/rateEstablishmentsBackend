using System.ComponentModel.DataAnnotations.Schema;

namespace dxz.Models
{
    public class Photo
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string File { get; set; }


        [ForeignKey(nameof(Establishment))]
        public int EstablishmentFK { get; set; }

    }
}
