using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Establishment
    {

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Email do Estabelecimento
        /// </summary>
        [EmailAddress(ErrorMessage = "O {0} não está corretamente escrito")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [RegularExpression("[a-z._0-9]+@[a-z._0-9].com", ErrorMessage = "O {0} tem de ser válido ex:xxxx@zzzz.com")]
        [StringLength(40)]
        public string Email { get; set; }


        /// <summary>
        /// Password do Estabelecimento 
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8}$", ErrorMessage = "A {0} deve ter pelo menos 8 caractéres ,conter pelo menos uma letra minúscula, uma letra maiúscula, um dígito e um carácter especial")]
        public string Password { get; set; }

        /// <summary>
        /// Nome do Estabelecimento
        /// </summary>
        [Required(ErrorMessage ="O {0} é de preenchemento obrigatório")]
        [StringLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Cidade do Estabelcimento
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchemento obrigatório")]
        [StringLength(100)]
        public string City { get; set; }

        /// <summary>
        /// Endereço do estabelecimento
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchemento obrigatório")]
        [StringLength(100)]
        public string Address { get; set; }

        /// <summary>
        /// Número de telemóvel do estabelecimento
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name = "Telemóvel")]
        [StringLength(9, MinimumLength = 9,
                    ErrorMessage = "O {0} deve ter {1} dígitos")]
        [RegularExpression("9[1236][0-9]{7}",
                    ErrorMessage = "O número de {0} deve começar por 91, 92, 93 ou 96, e ter 9 dígitos")]
        public string Phone { get; set; }

        /// <summary>
        /// Tipo de Estabelecimento
        /// </summary>
        public establishmentType TypeEstablishment { get; set; }

        public enum establishmentType {
            Restaurante,
            Café,
            Bar,
            Hotel
        }


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
