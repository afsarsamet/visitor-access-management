using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    [Authorize(Roles = "Admin")]
    public class YoneticiController : Controller
    {
        private readonly dbContextClass _context;

        public YoneticiController(dbContextClass context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult OnayBekleyenler()
        {
            
            var talepler = _context.FirmaTalepleri
                .Where(t => t.OnaylandiMi == null)
                .OrderBy(t => t.TalepTarihi)
                .ToList();
            return View(talepler);
        }
        [HttpPost]
        public IActionResult TalebiOnayla(int id)
        {
            var talep = _context.FirmaTalepleri.Find(id);
            if (talep == null) return NotFound();

            
            var yeniFirma = new Firma
            {
                FirmaAdi = talep.FirmaAdi,
                AktifMi = true
               
            };
            _context.Firmalar.Add(yeniFirma);

            
            talep.OnaylandiMi = true;

            _context.SaveChanges();
            return Json(new { success = true }); 
        }
        [HttpPost]
        public IActionResult TalebiReddet(int id)
        {
            var talep = _context.FirmaTalepleri.Find(id);
            if (talep == null) return NotFound();

            
            talep.OnaylandiMi=false;

            _context.SaveChanges();
            return Json(new { success = true });
        }

    }
}
