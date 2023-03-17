using Microsoft.AspNetCore.Mvc;
using PetStok.Entities;
using PetStok.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Net.Http.Headers;
using PetStok.Entities.DTOs;

namespace PetStok.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<ProductDTO> productDTOs = new();
            productDTOs = GetProducts("0000000000000");
            ViewBag.Date = DateTime.Now;
            ViewBag.productDTOs = productDTOs;
            var adlar = productDTOs.DistinctBy(p => p.ad).Select(p => p.ad);
            ViewBag.adlar = adlar;
            ViewBag.ad = adlar.FirstOrDefault() == null ? "" : adlar.FirstOrDefault();
            ViewBag.barkod = "";
            return View();
        }
        [HttpPost]
        public IActionResult Guncelle(string barkod)
        {
            List<ProductDTO> productDTOs = new();
            productDTOs = GetProducts(barkod);
            ViewBag.Date = DateTime.Now;
            ViewBag.productDTOs = productDTOs;
            var adlar = productDTOs.DistinctBy(p => p.ad).Select(p => p.ad);
            ViewBag.adlar = adlar;
            ViewBag.ad = adlar.First();
            ViewBag.barkod = barkod;
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public List<ProductDTO> GetProducts(string text)
        {
            ////Offline veri ile çalışma kodu
            //var jsonPath = @"C:\Users\mehme\Desktop\petresponse.json";
            //var myJsonString = System.IO.File.ReadAllText(jsonPath);
            //var products = JsonSerializer.Deserialize<List<Product>>(myJsonString);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://193.53.103.155:8090");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("text/plain"));
            HttpResponseMessage response = client.GetAsync("/api/Genel/getUrunsdetay/" + text).Result;
            List<Product> products = new();
            var productDTOs = new List<ProductDTO>();
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var foo = response.Content.ReadAsStringAsync().Result;
                products = JsonSerializer.Deserialize<List<Product>>(foo);
                //products = response.Content.ReadAsAsync<List<Product>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                client.Dispose();
                return productDTOs;
            }
            client.Dispose();

            foreach (var product in products)
            {
                // rambo 22
                StokKaydi oncekiKayit = new();
                List<StokKaydi> kayitlar = new();
                if (product.stokList != null)
                {
                    kayitlar = JsonSerializer.Deserialize<List<StokKaydi>>(product.stokList);
                }
                else
                {
                    productDTOs.Add(new ProductDTO
                    {
                        ad = product.ad,
                        barkod = product.barkod,
                        fiyat = product.fiyat,
                        id = product.id,
                        stok = product.stok,
                        tarih = product.tarih,
                        url = product.url,
                        urunAd = product.urunAd,
                        oncekiStok = oncekiKayit,
                        sonrakiStok = new StokKaydi { stok = product.stok, tarih = product.tarih.HasValue ? (DateTime)product.tarih : new DateTime() }
                    });
                }
                foreach (StokKaydi kayit in kayitlar)
                {
                    if (oncekiKayit.tarih.Date == kayit.tarih.Date)
                    {
                        continue;
                    }
                    productDTOs.Add(new ProductDTO
                    {
                        ad = product.ad,
                        barkod = product.barkod,
                        fiyat = product.fiyat,
                        id = product.id,
                        stok = product.stok,
                        tarih = product.tarih,
                        url = product.url,
                        urunAd = product.urunAd,
                        oncekiStok = oncekiKayit,
                        sonrakiStok = kayit
                    });
                    oncekiKayit = kayit;
                }
            }


            //productDTOs = productDTOs.GroupBy(car => new { car.barkod , car.ad,car.tarih}).Select(g => g.First()).ToList();

            return productDTOs;
        }
    }
}
