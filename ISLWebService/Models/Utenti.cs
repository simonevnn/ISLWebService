using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("utenti")]
    public class Utenti
    {
        [Key]
        [Column("username")]
        [MinLength(5, ErrorMessage = "Nome utente troppo corto.")]
        [MaxLength(30, ErrorMessage = "Il nome utente supera la lunghezza massima.")]
        public string Username { get; set; }

        [Required]
        [Column("password")]
        [MinLength(8, ErrorMessage = "Password troppo corta.")]
        [MaxLength(64)]
        public string Password { get; set; }

        [Required]
        [Column("nome")]
        [MaxLength(30, ErrorMessage = "Il nome supera la lunghezza massima.")]
        public string Nome { get; set; }

        [Required]
        [Column("cognome")]
        [MaxLength(30, ErrorMessage = "Il cognome supera la lunghezza massima.")]
        public string Cognome { get; set; }

        [Column("data_nascita")]
        public DateTime? DataNascita { get; set; }

        [Column("luogo_nascita")]
        [MaxLength(100, ErrorMessage = "Il luogo di nascita supera la lunghezza massima.")]
        public string? LuogoNascita { get; set; }

        [Column("telefono")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Il numero di telefono deve essere esattamente di 10 caratteri.")]
        public string? Telefono { get; set; }

        //FK
        [Required]
        [Column("codice_magazzino")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice del magazzino deve essere esattamente di 3 caratteri.")]
        public string CodiceMagazzino { get; set; }

        //LEGAMI
        [ForeignKey("CodiceMagazzino")]
        public Magazzini? Magazzino { get; set; }

        public IEnumerable<UtentiRuoli>? UtentiRuoli { get; set; }
        public IEnumerable<UtentiTask>? UtentiTask { get; set; }

    }
}
