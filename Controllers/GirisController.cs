using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    public class GirisController : Controller
    {
        private readonly dbContextClass _context; 

        public GirisController(dbContextClass context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string SicilNo, string sifre)
        {
            var personel = _context.Personel.FirstOrDefault(p => p.SicilNo == SicilNo && p.AktifMi && p.rol=="Güvenlik");

            if (personel != null && BCrypt.Net.BCrypt.Verify(sifre, personel.sifre ))
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, personel.SicilNo),
            new Claim("AdSoyad", personel.AdSoyad),
            new Claim(ClaimTypes.Role, personel.rol) 
        };

                var kimlik = new ClaimsIdentity(claims, "KardemirGuvenlik");
                var principal = new ClaimsPrincipal(kimlik);

                await HttpContext.SignInAsync("KardemirGuvenlik", principal);
                return RedirectToAction("Index", "visitorLogs");
            }

            ViewBag.Hata = "Sicil No veya Şifre hatalı!";
            return View();
        }

        public async Task<IActionResult> CikisYap()
        { 
            await HttpContext.SignOutAsync("KardemirGuvenlik");

            return RedirectToAction("Index", "Giris");
        }
    }
}