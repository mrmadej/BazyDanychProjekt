using BazyDanychProjekt.Data;
using BazyDanychProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazyDanychProjekt.Controllers
{
    public class OpinieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OpinieController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IActionResult Szczegoly(int hotelId)
        //{
        //    // Pobierz hotel i jego opinie wraz z informacjami o użytkownikach
        //    var hotel = _context.Hotele
        //        .Include(h => h.Opinie)
        //            .ThenInclude(o => o.Uzytkownik)  // Pobierz informacje o użytkowniku
        //        .FirstOrDefault(h => h.Id == hotelId);

        //    if (hotel == null)
        //    {
        //        return RedirectToAction("Blad", "Home");
        //    }

        //    // Przekazujesz model hotelu do widoku Szczegoly
        //    return View(hotel);
        //}

        [HttpPost]
        public IActionResult DodajOpinie(Opinia opinia)
        {
            //if (ModelState.IsValid)
            //{

                // Pobierz Id użytkownika z sesji
                var userId = HttpContext.Session.GetString("UserId");
                opinia.Data = DateTime.Now;
                opinia.UzytkownikId = userId;

                // Dodanie opinii do bazy danych
                _context.Opinia.Add(opinia);
                _context.SaveChanges();

            // Przekierowanie z powrotem do widoku Szczegoly
            return RedirectToAction("Szczegoly", "Hotele", new { id = opinia.HotelId });
            //}

            // W przypadku błędnych danych, powrót do widoku Szczegoly
            var hotel = _context.Hotele
                .Include(h => h.Opinie)
                .FirstOrDefault(h => h.Id == opinia.HotelId);

            return RedirectToAction("Index", "Hotele");
        }
    }
}
