using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Establishment
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public EstablishmentType EstablishmentType { get; set; }

    }

    public enum EstablishmentType
    {
        Restaurante, 
        Café, 
        Bar, 
        Hotel
    }
}
