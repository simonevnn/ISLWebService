using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("bancali_task")]
    public class BancaliTask
    {
        [Key]
        [Column("numero_seriale_bancale")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Il numero seriale del bancale deve essere esattamente di 13 caratteri.")]
        public string NumeroSerialeBancale { get; set; }

        [Key]
        [Column("id_task")]
        public uint IdTask { get; set; }

        [ForeignKey("NumeroSerialeBancale")]
        public virtual Bancali? Bancale { get; set; }

        [ForeignKey("IdTask")]
        public virtual Compiti? Task { get; set; }
    }
}
