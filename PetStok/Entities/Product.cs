using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PetStok.Entities
{
    public class Product
    {
        public int id { get; set; }
        public string? ad { get; set; }
        public string? url { get; set; }
        public string? barkod { get; set; }
        public int stok { get; set; }
        public double fiyat { get; set; }
        public DateTime? tarih { get; set; }
        public string? urunAd { get; set; }
        public string? stokList { get ; set; }
    }
}
