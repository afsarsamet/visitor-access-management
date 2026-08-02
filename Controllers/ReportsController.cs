using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ReportsController : Controller
    {
        private readonly dbContextClass _context;

        public ReportsController(dbContextClass context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? baslangic, DateTime? bitis, bool sadeceAsanlar = false)
        {
            
            var query = _context.visitorLog.Include(x => x.Firma).AsQueryable();

            
            if (baslangic.HasValue)
            {
                query = query.Where(x => x.girisZamani >= baslangic.Value);
            }

            
            if (bitis.HasValue)
            {
                
                var bitisGecesi = bitis.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.girisZamani <= bitisGecesi);
            }

           
            if (sadeceAsanlar)
            {
                query = query.Where(x => x.asimVarMi == true);
            }

            
            var raporListesi = await query.OrderByDescending(x => x.girisZamani).ToListAsync();

            
            ViewBag.Baslangic = baslangic?.ToString("yyyy-MM-dd");
            ViewBag.Bitis = bitis?.ToString("yyyy-MM-dd");
            ViewBag.SadeceAsanlar = sadeceAsanlar;

            return View(raporListesi);
        }
        [HttpGet]
        public async Task<IActionResult> ExportExcel(DateTime? baslangic, DateTime? bitis, bool sadeceAsanlar = false)
        {
            
            var query = _context.visitorLog.Include(x => x.Firma).AsQueryable();

            if (baslangic.HasValue)
                query = query.Where(x => x.girisZamani >= baslangic.Value);

            if (bitis.HasValue)
            {
                var bitisGecesi = bitis.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.girisZamani <= bitisGecesi);
            }

            if (sadeceAsanlar)
                query = query.Where(x => x.asimVarMi == true);

            var raporListesi = await query.OrderByDescending(x => x.girisZamani).ToListAsync();

           
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Araç Raporu");

              
                var basliklar = new string[] { "Plaka", "Firma Adı", "Sürücü Ad Soyad", "Telefon No", "Giriş Saati", "Çıkış Saati", "Durum" };
                for (int i = 0; i < basliklar.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = basliklar[i];
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#1f4e78"); 
                    worksheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.White;
                }

                
                int satir = 2; 
                foreach (var arac in raporListesi)
                {
                    worksheet.Cell(satir, 1).Value = arac.plaka;
                    worksheet.Cell(satir, 2).Value = arac.Firma?.FirmaAdi ?? "Belirtilmemiş";
                    worksheet.Cell(satir, 3).Value = arac.adSoyad;
                    worksheet.Cell(satir, 4).Value = arac.telefonNo;
                    worksheet.Cell(satir, 5).Value = arac.girisZamani.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cell(satir, 6).Value = arac.cikisZamani.HasValue ? arac.cikisZamani.Value.ToString("dd.MM.yyyy HH:mm") : "İçeride";

                    worksheet.Cell(satir, 7).Value = arac.asimVarMi ? "Kural İhlali" : "Sorunsuz";

                    satir++;
                }

                
                worksheet.Columns().AdjustToContents();

                
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Kardemir_Arac_Raporu_{DateTime.Now:dd_MM_yyyy}.xlsx");
                }
            }
        }
    }
}