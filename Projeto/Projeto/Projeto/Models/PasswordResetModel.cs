using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class PasswordResetModel
    {
        /// <summary>
        /// Password do Estabelecimento 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Confirmar Password do Estabelecimento 
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string ConfirmarPassword { get; set; }

    }
}
