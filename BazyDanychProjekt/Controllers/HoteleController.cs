using Microsoft.AspNetCore.Mvc;
using BazyDanychProjekt.Models;
using System.Collections.Generic;
using BazyDanychProjekt.Services;

namespace BazyDanychProjekt.Controllers
{
    public class HoteleController : Controller
    {
        private readonly IHoteleService _hoteleService;

        public HoteleController(IHoteleService hoteleService)
        {
            _hoteleService = hoteleService;
        }

        public IActionResult Index()
        {
            // Pobierz listę hoteli z serwisu
            List<Hotel> hotele = _hoteleService.PobierzHotele();

            // Przekaż listę hoteli do widoku
            return View(hotele);
        }

        public IActionResult Szczegoly(int id)
        {
            // Pobierz szczegóły hotelu o danym id
            Hotel hotel = _hoteleService.PobierzHotel(id);

            // Sprawdź, czy TempData zawiera informację o błędzie
            if (TempData.ContainsKey("BladRezerwacji"))
            {
                ViewBag.BladRezerwacji = TempData["BladRezerwacji"];
            }
            if (TempData.ContainsKey("PomyslnaRezerwacja"))
            {
                ViewBag.PomyslnaRezerwacja = TempData["PomyslnaRezerwacja"];
            }

            if (hotel == null)
            {
                // Obsłuż brak hotelu o podanym id (np. przekierowanie do strony błędu)
                return RedirectToAction("Blad", "Home");
            }

            // Przekaż szczegóły hotelu do widoku
            return View(hotel);
        }
    }
}
