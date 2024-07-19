using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("magazzini")]
    public class Magazzini
    {
        [Key]
        [Column("codice_magazzino")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice del magazzino deve essere esattamente di 3 caratteri.")]
        public string CodiceMagazzino { get; set; }

        [Column("indirizzo")]
        [MaxLength(100, ErrorMessage = "L'indirizzo del magazzino supera la lunghezza consentita.")]
        public string? Indirizzo { get; set; }

        [Column("citta")]
        [MaxLength(30, ErrorMessage = "La città del magazzino supera la lunghezza consentita.")]
        public string? Citta { get; set; }
        
        //LEGAMI
        public virtual IEnumerable<Aree>? Aree { get; set; }
        public virtual IEnumerable<Utenti>? Utenti { get; set; }
    }
}
