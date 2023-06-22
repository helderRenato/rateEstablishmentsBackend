using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class LoginModel
    {
        /// <summary>
        /// Email do Estabelecimento
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// Password do Estabelecimento 
        /// </summary>
        public string Password { get; set; }

    }
}
