namespace Projeto.Models
{
    public class CommentTransportModel
    {

        public string Text { get; set; }
        public string? Response { get; set; }
        public int UserFK { get; set; }
        public int EstablishmentFK { get; set; }
    }
}
