using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class PasswordResetModel
    {
        /// <summary>
        /// Password do Estabelecimento 
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "A {0} deve ter pelo menos 8 caractéres ,conter pelo menos uma letra minúscula, uma letra maiúscula, um dígito e um carácter especial")]
        public string Password { get; set; }

        /// <summary>
        /// Password do Estabelecimento 
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string ConfirmarPassword { get; set; }

    }
}
