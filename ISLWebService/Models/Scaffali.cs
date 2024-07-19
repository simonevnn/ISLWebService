using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("scaffali")]
    public class Scaffali
    {
        [Key]
        [Column("codice_scaffale")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dello scaffale deve essere esattamente di 3 caratteri.")]
        public string CodiceScaffale { get; set; }

        [Key]
        [Column("codice_area")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dell'area deve essere esattamente di 3 caratteri.")]
        public virtual string CodiceArea { get; set; }

        [Key]
        [Column("codice_magazzino")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice del magazzino deve essere esattamente di 3 caratteri.")]
        public virtual string CodiceMagazzino { get; set; }

        //LEGAMI
        [ForeignKey("CodiceArea")]
        public virtual Aree? Area { get; set; }
        public virtual IEnumerable<Posti>? Posti { get; set; }
    }
}
