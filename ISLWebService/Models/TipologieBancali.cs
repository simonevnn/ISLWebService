using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("tipologie_bancali")]
    public class TipologieBancali
    {
        [Key]
        [Column("codice_tipologia")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Il codice della tipologia deve essere esattamente di 5 caratteri.")]
        public string CodiceTipologia { get; set; }

        [Required]
        [Column("descrizione")]
        [MaxLength(100, ErrorMessage = "La descrizione della tipologia supera la lunghezza consentita.")]
        public string Descrizione { get; set; }

        //LEGAMI
        public virtual IEnumerable<Bancali>? Bancali { get; set; }
    }
}
