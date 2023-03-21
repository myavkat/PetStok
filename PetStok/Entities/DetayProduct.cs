namespace PetStok.Entities
{
    public class DetayProduct
    {
        public string basStok { get; set; }
        public string bitStok { get; set; }
        public string stokFark { get; set; }
        public string basStokTarih { get; set; }
        public string bitStokTarih { get; set; }
        public int id { get; set; }
        public string ad { get; set; }
        public string url { get; set; }
        public string barkod { get; set; }
        public int stok { get; set; }
        public double fiyat { get; set; }
        public DateTime? tarih { get; set; }
        public string urunAd { get; set; }
        public string stokList { get; set; }
        public bool populer { get; set; }
        public int totalpage { get; set; }

    }
}
