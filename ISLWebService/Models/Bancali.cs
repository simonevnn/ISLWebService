using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("bancali")]
    public class Bancali
    {
        [Key]
        [Column("numero_seriale")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Il numero seriale del bancale deve essere esattamente di 13 caratteri.")]
        public string? NumeroSeriale { get; set; }

        [Required]
        [Column("altezza")]
        [Range(0.00, 999.99, ErrorMessage = "L'altezza massima del bancale può essere espressa in numeri da 0 a 999.99")]
        public double Altezza { get; set; }

        [Required]
        [Column("larghezza")]
        [Range(0.00, 999.99, ErrorMessage = "La larghezza massima del bancale può essere espressa in numeri da 0 a 999.99")]
        public double Larghezza { get; set; }

        [Required]
        [Column("lunghezza")]
        [Range(0.00, 999.99, ErrorMessage = "La lunghezza massima del bancale può essere espressa in numeri da 0 a 999.99")]
        public double Lunghezza { get; set; }

        [Required]
        [Column("peso")]
        [Range(0.00, 999.99, ErrorMessage = "Il peso massimo del bancale può essere espresso in numeri da 0 a 999.99")]
        public double Peso { get; set; }

        [Required]
        [Column("carico_speciale")]
        public bool CaricoSpeciale { get; set; }

        [Column("data_scadenza")]
        public DateTime? DataScadenza { get; set; }

        [Column("numero_lotto")]
        public int? NumeroLotto { get; set; }

        [Column("mittente")]
        [MaxLength(100, ErrorMessage = "La lunghezza del mittente supera quella consentita.")]
        public string? Mittente { get; set; }

        //FOREIGN KEY
        [Column("codice_posto")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Il codice del posto deve essere esattamente di 13 caratteri.")]
        public string? CodicePosto { get; set; }

        [Column("codice_scaffale")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dello scaffale deve essere esattamente di 3 caratteri.")]
        public string? CodiceScaffale { get; set; }

        [Column("codice_area")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dell'area deve essere esattamente di 3 caratteri.")]
        public string? CodiceArea { get; set; }

        [Column("codice_magazzino")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice del magazzino deve essere esattamente di 3 caratteri.")]
        public string? CodiceMagazzino { get; set; }

        [Column("tipologia_carico")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Il codice della tipologia deve essere esattamente di 5 caratteri.")]
        public string? TipologiaCarico { get; set; }

        //LEGAMI
        [ForeignKey("CodicePosto")]
        public virtual Posti? Posto { get; set; }

        [ForeignKey("TipologiaCarico")]
        public virtual TipologieBancali? TipologiaBancale { get; set; }

        public virtual IEnumerable<BancaliTask>? BancaliTask { get; set; }

    }
}
