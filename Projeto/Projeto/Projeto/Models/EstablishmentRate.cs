using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class AvaliacaoEstabelecimento
    {

        public int Id { get; set; }

        public int Estrelas { get; set; }


        [ForeignKey(nameof(User))]
        public int UserFK { get; set; }

        [ForeignKey(nameof(Estabelecimento))]
        public int EstabelecimentoFK { get; set; }
    }
}
