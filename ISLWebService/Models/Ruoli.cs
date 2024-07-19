using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISLWebService.Models
{
    [Table("ruoli")]
    public class Ruoli
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }

        [Required]
        [Column("descrizione")]
        [MaxLength(100, ErrorMessage = "La descrizione del ruolo supera la lunghezza consentita.")]
        public string Descrizione { get; set; }

        //LEGAMI
        public IEnumerable<UtentiRuoli>? UtentiRuoli { get; set; }
        public IEnumerable<RuoliTask>? RuoliTask { get; set; }
    }
}
