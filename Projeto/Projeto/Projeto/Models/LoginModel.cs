using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class LoginModel
    {
        /// <summary>
        /// Email do Estabelecimento
        /// </summary>
        [EmailAddress(ErrorMessage = "O {0} não está corretamente escrito")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [RegularExpression("[a-z._0-9]+@[a-z]+.com", ErrorMessage = "O {0} tem de ser válido ex:xxxx@zzzz.com")]
        [StringLength(40)]
        public string Email { get; set; }


        /// <summary>
        /// Password do Estabelecimento 
        /// </summary>
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        public string Password { get; set; }

    }
}
