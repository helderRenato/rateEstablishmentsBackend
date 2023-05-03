using System.ComponentModel.DataAnnotations.Schema;

namespace Projeto.Models
{
    public class Estabelecimento
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Cidade { get; set; }

        public string Morada { get; set; }

        public string TipoDeEstabelecimento { get; set; }

        [ForeignKey(nameof(User))]
        public int IdUser { get; set; }
    }
}
