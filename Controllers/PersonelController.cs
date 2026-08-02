using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonelController : Controller
    {
        private readonly dbContextClass _context;

        public PersonelController(dbContextClass context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Personel yeniPersonel)
        {
            if (ModelState.IsValid)
            {
                
                yeniPersonel.sifre = BCrypt.Net.BCrypt.HashPassword(yeniPersonel.sifre);

                _context.Personel.Add(yeniPersonel);
                _context.SaveChanges();

                return RedirectToAction("Index", "visitorLogs"); 
            }
            return View(yeniPersonel);
        }
    }
}