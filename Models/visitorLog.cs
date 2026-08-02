namespace WebApplication1.Models
{
    public class visitorLog
    {
        public int id { get; set; }
        public required string adSoyad { get; set; }
        public int FirmaId { get; set; }
        public Firma? Firma { get; set; }
       
        public required string ziyaretNedeni { get; set; }
        public required int kacKisi { get; set; }
        public required string plaka { get; set; }
        public DateTime girisZamani { get; set; }
        public DateTime? cikisZamani { get; set; }
        public bool asimVarMi { get; set; }
        public required string telefonNo { get; set; }
        public bool? ciktiMi { get; set; }
        public string? cikisVerenAdSoyad { get; set; }
        public string? cikisVerenSicilNo { get; set; }
    }
}
