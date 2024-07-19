namespace ISLWebService.Dto
{
    public class UtentiDto
    {
        public string Username { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public DateTime DataNascita { get; set; }
        public string LuogoNascita { get; set; }
        public string Telefono { get; set; }
        public string Magazzino { get; set; }
        public IEnumerable<string> Ruoli { get; set; }

    }
}
