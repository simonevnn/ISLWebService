using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("ruoli_task")]
    public class RuoliTask
    {
        [Key]
        [Column("id_ruolo")]
        public uint IdRuolo { get; set; }

        [Key]
        [Column("id_task")]
        public uint IdTask { get; set; }

        [ForeignKey("IdRuolo")]
        public virtual Ruoli? Ruolo { get; set; }

        [ForeignKey("IdTask")]
        public virtual Compiti? Task { get; set; }
    }
}
