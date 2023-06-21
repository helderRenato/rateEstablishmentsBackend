using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Projeto.Models
{
    public class EstablishmentTransportModel
    {


        /// <summary>
        /// Email do Estabelecimento
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// Password do Estabelecimento 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Nome do Estabelecimento
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// Cidade do Estabelcimento
        /// </summary>

        public string City { get; set; }

        /// <summary>
        /// Endereço do estabelecimento
        /// </summary>

        public string Address { get; set; }

        public IFormFile File { get; set; }

        /// <summary>
        /// Tipo de Estabelecimento
        /// </summary>
        public Establishment.establishmentType TypeEstablishment { get; set; }

        

    }

}

