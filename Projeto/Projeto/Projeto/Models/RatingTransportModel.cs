using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class RatingTransportModel
    {

        
        public int Id { get; set; }

        
        public int Stars { get; set; }

        
        public int UserFK { get; set; }
        public User? User { get; set; } 

        
        public int EstablishmentFK { get; set; }
        public Establishment? Establishment { get; set; } 

    }
}
