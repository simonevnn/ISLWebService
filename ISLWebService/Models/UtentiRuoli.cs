using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("utenti_ruoli")]
    public class UtentiRuoli
    {
        [Key]
        [Column("username")]
        [MinLength(5, ErrorMessage = "Nome utente troppo corto.")]
        [MaxLength(30, ErrorMessage = "Il nome utente supera la lunghezza massima.")]
        public string Username { get; set; }

        [Key]
        [Column ("id_ruolo")]
        public uint IdRuolo { get; set; }

        //LEGAMI
        [ForeignKey("Username")]
        public Utenti? Utente { get; set; }

        [ForeignKey("IdRuolo")]
        public Ruoli? Ruolo { get; set; }
    }
}
