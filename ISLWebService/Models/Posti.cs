using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("posti")]
    public class Posti
    {
        [Required]
        [Column("max_altezza")]
        [Range(0.00, 999.99, ErrorMessage = "L'altezza massima del posto può essere espressa in numeri da 0 a 999.99")]
        public double MaxAltezza { get; set; }

        [Required]
        [Column("max_larghezza")]
        [Range(0.00, 999.99, ErrorMessage = "La larghezza massima del posto può essere espressa in numeri da 0 a 999.99")]
        public double MaxLarghezza { get; set; }

        [Required]
        [Column("max_lunghezza")]
        [Range(0.00, 999.99, ErrorMessage = "La lunghezza massima del posto può essere espressa in numeri da 0 a 999.99")]
        public double MaxLunghezza { get; set; }

        [Required]
        [Column("max_peso")]
        [Range(0.00, 999.99, ErrorMessage = "Il peso massimo del posto può essere espresso in numeri da 0 a 999.99")]
        public double MaxPeso { get; set; }

        [Required]
        [Column("carico_speciale")]
        public bool CaricoSpeciale { get; set; }

        [Key]
        [Column("codice_posto")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Il codice del posto deve essere esattamente di 13 caratteri.")]
        public string CodicePosto { get; set; }
        
        [Key]
        [Column("codice_scaffale")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dello scaffale deve essere esattamente di 3 caratteri.")]
        public virtual string CodiceScaffale { get; set; }

        [Key]
        [Column("codice_area")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dell'area deve essere esattamente di 3 caratteri.")]
        public virtual string CodiceArea { get; set; }

        [Key]
        [Column("codice_magazzino")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice del magazzino deve essere esattamente di 3 caratteri.")]
        public virtual string CodiceMagazzino { get; set; }

        //LEGAMI
        [ForeignKey("CodiceScaffale")]
        public virtual Scaffali? Scaffale { get; set; }
        public virtual Bancali? Bancale { get; set; }
        public virtual IEnumerable<Compiti>? TaskProvenienza { get; set; }
        public virtual IEnumerable<Compiti>? TaskDestinazione { get; set; }
    }
}
