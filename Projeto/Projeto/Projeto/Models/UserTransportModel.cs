using System.ComponentModel.DataAnnotations;

namespace Projeto.Models
{
    public class UserTransportModel
    {


        /// <summary>
        /// Username do Utilizador 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email do Utilizador
        /// </summary>
        [StringLength(40)]
        public string Email { get; set; }


        /// <summary>
        /// Password do Utilizador
        /// </summary>
        public string Password { get; set; }
    }
}
