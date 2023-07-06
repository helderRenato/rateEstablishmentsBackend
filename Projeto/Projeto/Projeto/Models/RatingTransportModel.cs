using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace Projeto.Models
{
    public class RatingTransportModel
    {
        //A avaliação de um utilizador ao estabelecimento deve conter um comentário obrigatoriamente

        /// <summary>
        /// Texto 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Estrelas
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// Resposta ao comentário
        /// </summary>
        public string? Response { get; set; }

        /// <summary>
        /// Id do utilizador que faz o rating
        /// </summary>
        public int UserFK { get; set; }



    }
}
