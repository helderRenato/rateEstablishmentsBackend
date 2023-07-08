using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace Projeto.Models
{
    public class RatingTransportModel
    {
        /// <summary>
        /// Estrelas
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// Id do utilizador que faz o rating
        /// </summary>
        public int UserFK { get; set; }

    }
}
