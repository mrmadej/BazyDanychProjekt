using Microsoft.AspNetCore.Mvc;
using BazyDanychProjekt.Models;
using System.Collections.Generic;
using BazyDanychProjekt.Services;
using Microsoft.AspNetCore.Authorization;

namespace BazyDanychProjekt.Controllers
{
    [Authorize]
    public class HoteleController : Controller
    {
        private readonly IHoteleService _hoteleService;

        public HoteleController(IHoteleService hoteleService)
        {
            _hoteleService = hoteleService;
        }

        public IActionResult Index()
        {
            List<Hotel> hotele = _hoteleService.PobierzHotele();

            return View(hotele);
        }

        public IActionResult Szczegoly(int id)
        {
            Hotel hotel = _hoteleService.PobierzHotel(id);

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
                return RedirectToAction("Blad", "Home");
            }

            return View(hotel);
        }
    }
}
