using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Controllers
{
    [Authorize]

    public class visitorLogsController : Controller
    {
        private readonly dbContextClass _context;

        public visitorLogsController(dbContextClass context) { _context = context; }
        public IActionResult Index()
        {
            
            var aracListesi = _context.visitorLog
                .Include(v => v.Firma)
                .Where(arac => arac.cikisZamani == null)
                .ToList();

            return View(aracListesi);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(visitorLog gelenArac)
        {
            gelenArac.girisZamani = DateTime.Now;
            _context.visitorLog.Add(gelenArac);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> CikisYap(int id)
        {
            var aracLog = await _context.visitorLog.FindAsync(id);
            if (aracLog == null)
            {
                return NotFound();
            }
            if (aracLog.cikisZamani != null)
            {
                return BadRequest();
            }


            var aktifPersonelSicilNo = User.Identity?.Name;
            var aktifPersonelAdSoyad = User.Claims.FirstOrDefault(c => c.Type == "AdSoyad")?.Value;

           
            aracLog.cikisZamani = DateTime.Now;
            aracLog.ciktiMi = true; 
            aracLog.cikisVerenSicilNo = aktifPersonelSicilNo;
            aracLog.cikisVerenAdSoyad = aktifPersonelAdSoyad;

            
            _context.Update(aracLog);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        


        [HttpGet]
        public IActionResult Details(int id)
        {
            
            var arac = _context.visitorLog
                .Include(v => v.Firma)
                .FirstOrDefault(v => v.id == id);

            if (arac == null) { return RedirectToAction("Index"); }

            return View(arac);
        }
    }
}
