using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace WebApplication1.Controllers
{
    public class YoneticiAuthController : Controller
    {
        private readonly dbContextClass _context;

        public YoneticiAuthController(dbContextClass context)
        {
            _context = context;
        }


        [Route("YoneticiGirisi")]
        [HttpGet]
        public IActionResult Login()
        {
            
            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "visitorLogs");
            }
            return View();
        }

        
        [Route("YoneticiGirisi")]
        [HttpPost]
        public async Task<IActionResult> Login(string SicilNo, string sifre)
        {
            var yonetici = _context.Personel.FirstOrDefault(y => y.SicilNo == SicilNo);

            if (yonetici != null && BCrypt.Net.BCrypt.Verify(sifre, yonetici.sifre) && yonetici.rol=="Admin" && yonetici.AktifMi)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, yonetici.SicilNo),
            new Claim("AdSoyad", yonetici.AdSoyad),
            new Claim(ClaimTypes.Role, yonetici.rol)
        };

                var identity = new ClaimsIdentity(claims, "KardemirGuvenlik");

               
                var principal = new ClaimsPrincipal(identity);

               
                await HttpContext.SignInAsync("KardemirGuvenlik", principal);

              
                return RedirectToAction("Index", "visitorLogs");
            }

         
            ViewBag.Hata = "Yetkisiz erişim! Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        
        [Route("YoneticiCikis")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            
            await HttpContext.SignOutAsync("KardemirGuvenlik");

           
            return RedirectToAction("Index", "visitorLogs");
        }
    }
}