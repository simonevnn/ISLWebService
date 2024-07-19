using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("task")]
    public class Compiti
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Required]
        [Column("nome")]
        [MaxLength(30, ErrorMessage = "Il nome della task supera la lunghezza consentita.")]
        public string Nome { get; set; }

        [Required]
        [Column("descrizione")]
        public string Descrizione { get; set; }

        [Column("tipologia")]
        public string? Tipologia { get; set; }

        [Required]
        [Column("status")]
        public byte Status { get; set; }

        [Column("data_ora_scadenza")]
        public DateTime? DataOraScadenza { get; set; }

        [Column("data_ora_completamento")]
        public DateTime? DataOraCompletamento { get; set; }

        [Required]
        [Column("urgenza")]
        public byte Urgenza { get; set; }

        [Column("info_ulteriori")]
        public string? InfoUlteriori { get; set; }

        [Column("codice_posto_provenienza")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Il codice del posto di provenienza deve essere esattamente di 13 caratteri.")]
        public string? CodicePostoProvenienza { get; set; }

        [Column("codice_scaffale_provenienza")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dello scaffale di provenienza deve essere esattamente di 3 caratteri.")]
        public string? CodiceScaffaleProvenienza { get; set; }

        [Column("codice_area_provenienza")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dell'area di provenienza deve essere esattamente di 3 caratteri.")]
        public string? CodiceAreaProvenienza { get; set; }

        [Column("codice_magazzino_provenienza")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice del magazzino di provenienza deve essere esattamente di 3 caratteri.")]
        public string? CodiceMagazzinoProvenienza { get; set; }

        [Column("codice_posto_destinazione")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Il codice del posto di destinazione deve essere esattamente di 13 caratteri.")]
        public string? CodicePostoDestinazione { get; set; }

        [Column("codice_scaffale_destinazione")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dello scaffale di destinazione deve essere esattamente di 3 caratteri.")]
        public string? CodiceScaffaleDestinazione { get; set; }

        [Column("codice_area_destinazione")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice dell'area di destinazione deve essere esattamente di 3 caratteri.")]
        public string? CodiceAreaDestinazione { get; set; }

        [Column("codice_magazzino_destinazione")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Il codice del magazzino di provenienza deve essere esattamente di 3 caratteri.")]
        public string? CodiceMagazzinoDestinazione { get; set; }

        //LEGAMI
        public virtual Posti? PostoProvenienza { get; set; }
        public virtual Posti? PostoDestinazione { get; set; }
        public virtual IEnumerable<UtentiTask>? UtentiTask { get; set; }
        public virtual IEnumerable<RuoliTask>? RuoliTask { get; set; }
        public virtual IEnumerable<BancaliTask>? BancaliTask { get; set; }
    }
}
