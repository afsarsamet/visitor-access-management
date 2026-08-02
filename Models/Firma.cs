using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Firma
    {
        [Key]
        public int FirmaId { get; set; }

        [Required]
        public string FirmaAdi { get; set; }

        public bool AktifMi { get; set; }
    }
}
