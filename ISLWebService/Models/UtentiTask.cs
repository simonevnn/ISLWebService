using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("utenti_task")]
    public class UtentiTask
    {
        [Key]
        [Column("username")]
        [MinLength(5, ErrorMessage = "Nome utente troppo corto.")]
        [MaxLength(30, ErrorMessage = "Il nome utente supera la lunghezza massima.")]
        public string Username { get; set; }

        [Key]
        [Column("id_task")]
        public uint IdTask { get; set; }

        [ForeignKey("Username")]
        public virtual Utenti? Utente { get; set; }

        [ForeignKey("IdTask")]
        public virtual Compiti? Task { get; set; }
    }
}
