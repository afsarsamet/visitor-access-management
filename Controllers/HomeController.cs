using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
         
        private readonly dbContextClass _context;
       

        public HomeController(dbContextClass context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var iceridekiAraclar = _context.visitorLog.Where(a => a.cikisZamani == null).ToList();

            ViewBag.ToplamIcerideki = iceridekiAraclar.Count;
            ViewBag.AsimYapanlar = iceridekiAraclar.Count(a => a.asimVarMi);
            ViewBag.KurallaraUygun = iceridekiAraclar.Count(a => !a.asimVarMi);
            return View();
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
    }
}
