using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("aree")]
    public class Aree
    {
        [Column("tipologia_area")]
        [MaxLength(100, ErrorMessage = "La tipologia dell'area supera la lunghezza consentita.")]
        public string? TipologiaArea { get; set; }

        [Key]
        [Column("codice_area")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dell'area deve essere esattamente di 3 caratteri.")]
        public string CodiceArea { get; set; }

        [Key]
        [Column("codice_magazzino")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice del magazzino deve essere esattamente di 3 caratteri.")]
        public virtual string CodiceMagazzino { get; set; }

        //LEGAMI
        [ForeignKey("CodiceMagazzino")]
        public virtual Magazzini? Magazzino { get; set; }
        public virtual IEnumerable<Scaffali>? Scaffali { get; set; }
    }
}
