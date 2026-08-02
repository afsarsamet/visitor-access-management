namespace WebApplication1.Models
{
    public class Personel
    {
        public int Id { get; set; }
        public required string AdSoyad { get; set; }
        public required string SicilNo { get; set; }
        public required string TCNo { get; set; }
        public required string TelefonNo { get; set; }
        public required bool AktifMi { get; set; }
        
        public required string sifre { get; set; }
        public string rol { get; set; } = "Güvenlik";
    }
}
