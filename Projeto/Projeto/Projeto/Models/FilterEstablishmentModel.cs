using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Projeto.Models
{
    public class FilterEstablishmentModel
    {

        /// <summary>
        /// Nome do Estabelecimento
        /// </summary>

        public string? Name { get; set; }

        /// <summary>
        /// Cidade do Estabelcimento
        /// </summary>

        public string? City { get; set; }


        /// <summary>
        /// Tipo de Estabelecimento
        /// </summary>
        public Establishment.establishmentType? TypeEstablishment { get; set; }


    }

}

