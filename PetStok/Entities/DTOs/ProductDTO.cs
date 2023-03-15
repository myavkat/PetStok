namespace PetStok.Entities.DTOs
{
    public class ProductDTO
    {
        public int id { get; set; }
        public string? ad { get; set; }
        public string? url { get; set; }
        public string? barkod { get; set; }
        public int stok { get; set; }
        public double fiyat { get; set; }
        public DateTime? tarih { get; set; }
        public string? urunAd { get; set; }
        public StokKaydi oncekiStok { get; set; }
        public StokKaydi sonrakiStok { get; set; }
    }
}
