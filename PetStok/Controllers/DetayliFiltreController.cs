using Microsoft.AspNetCore.Mvc;
using PetStok.Entities;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

namespace PetStok.Controllers
{
    public class DetayliFiltreController : Controller
    {
        public IActionResult Index()
        {
            return Filtrele();
        }

        public IActionResult Filtrele(bool json = false, bool populer = false, string ad = "hepsi", int page = 0, string barkod = "", DateTime? basTarih = null, DateTime? bitTarih = null)
        {
            basTarih ??= new DateTime().AddYears(1899);
            bitTarih ??= new DateTime().AddYears(2999);
            ViewBag.json = json; ViewBag.populer = populer; ViewBag.ad = ad; ViewBag.basTarih = basTarih.Value.ToString("yyyy-MM-dd"); ViewBag.bitTarih = bitTarih.Value.ToString("yyyy-MM-dd"); ViewBag.barkod = barkod;
            barkod ??= "";
            postModel contnt = new postModel { basTarih = basTarih.Value.ToString("yyyy-MM-dd"), bitTarih = bitTarih.Value.ToString("yyyy-MM-dd"), json = json, populer = populer, ad = ad, page = page, barkod = barkod };
            var jsonStr = JsonSerializer.Serialize(contnt);
            StringContent stringContent = new StringContent(jsonStr, Encoding.UTF8, "application/json");
            using (HttpClient client = new())
            {
                client.BaseAddress = new Uri("http://193.53.103.155:8090");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage dataResponse = client.PostAsync("/api/Genel/getJsonPopuler", stringContent).Result;
                if (dataResponse.IsSuccessStatusCode)
                {
                    ViewBag.detayProducts = dataResponse.Content.ReadAsAsync<List<DetayProduct>>().Result;
                }
                HttpResponseMessage adlarResponse = client.GetAsync("/api/Genel/getFirma").Result;
                if (adlarResponse.IsSuccessStatusCode)
                {
                    ViewBag.adlar = adlarResponse.Content.ReadAsAsync<List<a>>().Result;
                }
            }
            ViewBag.page = page;
            if (ViewBag.detayProducts.Count == 0)
            {
                ViewBag.totalPage = 5;
            }
            else
            {
                ViewBag.totalPage = ViewBag.detayProducts[0].totalpage;
            }
            return View("Index");
        }
        public class a { public string ad { get; set; } }
        public class postModel
        {
            public string ad { get; set; }
            public bool json { get; set; }
            public bool populer { get; set; }
            public int page { get; set; }
            public string barkod { get; set; }
            public string basTarih { get; set; }
            public string bitTarih { get; set; }
        }
    }
}
