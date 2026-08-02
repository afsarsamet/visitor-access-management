using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class FirmaTalep
    {
        [Key]
        public int Id { get; set; }
        public string FirmaAdi { get; set; }
        public DateTime TalepTarihi { get; set; }
        public bool? OnaylandiMi { get; set; }

       
        public string TalepEdenSicilNo { get; set; }
        public string TalepEdenAdSoyad { get; set; }


    }
}
