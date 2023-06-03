using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Username do Utilizador 
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [StringLength(50)]
        [RegularExpression("[a-z]+[0-9]", ErrorMessage = "O username apenas pode conter letras minúsculas")]
        public string Username { get; set; }

        /// <summary>
        /// Email do Utilizador
        /// </summary>
        [EmailAddress(ErrorMessage = "O {0} não está corretamente escrito")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [RegularExpression("[a-z._0-9]+@[a-z]+.com", ErrorMessage = "O {0} tem de ser válido ex:xxxx@zzzz.com")]
        [StringLength(40)]
        public string Email { get; set; }


        /// <summary>
        /// Password do Utilizador
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "A {0} deve ter pelo menos 8 caractéres ,conter pelo menos uma letra minúscula, uma letra maiúscula, um dígito e um carácter especial")]
        public string Password { get; set; }


        public ICollection<EstablishmentRate> ListEstablishmentRate { get; set; }

        public ICollection<CommentRate> ListCommentRate { get; set; }


        public User()
        {

            ListEstablishmentRate = new List<EstablishmentRate>();
            ListCommentRate = new List<CommentRate>();
        }
    }
}
