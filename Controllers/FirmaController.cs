using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class FirmaController : Controller
    {
        private readonly dbContextClass _context;

        
        public FirmaController(dbContextClass context)
        {
            _context = context;
        }

      
        public IActionResult Index()
        {
            var firmalar = _context.Firmalar.ToList(); 
            return View(firmalar); 
        }
        [HttpPost]
        public IActionResult YeniFirmaTalebi(string firmaAdi)
        {
            if (string.IsNullOrEmpty(firmaAdi)) return BadRequest();

            var yeniTalep = new FirmaTalep
            {
                FirmaAdi = firmaAdi,
                TalepTarihi = DateTime.Now,
                TalepEdenSicilNo = User.Identity.Name,
                TalepEdenAdSoyad= User.Claims.FirstOrDefault(c => c.Type == "AdSoyad")?.Value
            };


            _context.FirmaTalepleri.Add(yeniTalep);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult FirmaAra(string arananKelime)
        {
            
            if (string.IsNullOrWhiteSpace(arananKelime))
            {
                return Json(new { });
            }

            
            var bulunanFirmalar = _context.Firmalar
                .Where(f => f.FirmaAdi.StartsWith(arananKelime) && f.AktifMi == true)
                .OrderBy(f => f.FirmaAdi)
                .Take(10)
                .Select(f => new
                {
                    id = f.FirmaId,
                    text = f.FirmaAdi
                })
                .ToList();

            
            return Json(bulunanFirmalar);
        }
    }
}
