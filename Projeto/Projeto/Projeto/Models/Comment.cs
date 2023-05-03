namespace Projeto.Models
{
    public class Comentario
    {


        public Comentario()
        {
            ListaAvaliacaoComentario = new HashSet<AvaliacaoComentario>();
        }

        public int Id { get; set; }

        public Boolean Denunciado { get; set; }

        public string Texto { get; set;}

        public Boolean isAnswer { get; set;}

        public ICollection<AvaliacaoComentario> ListaAvaliacaoComentario { get; set; }
    }
}
