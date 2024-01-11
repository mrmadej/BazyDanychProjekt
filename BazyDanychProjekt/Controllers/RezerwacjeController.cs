using BazyDanychProjekt.Data;
using BazyDanychProjekt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazyDanychProjekt.Controllers
{
    public class RezerwacjeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Uzytkownik> _userManager;

        public RezerwacjeController(ApplicationDbContext context, UserManager<Uzytkownik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> DodajRezerwacje(RezerwacjeViewModel model)
        {
            //if (ModelState.IsValid)
            //{
                // Sprawdź dostępność pokoju w danym okresie
                bool czyPokojDostepny = await SprawdzDostepnoscPokoju(model.PokojId, model.DataPoczatek, model.DataKoniec);

                if (czyPokojDostepny)
                {
                // Dodaj rezerwację do bazy danych

                    var rezerwacja = new Rezerwacja
                    {
                        PokojId = model.PokojId,
                        UzytkownikId = HttpContext.Session.GetString("UserId"),
                        DataPoczatek = model.DataPoczatek,
                        DataKoniec = model.DataKoniec
                    };
                    
                    _context.Rezerwacja.Add(rezerwacja);
                    await _context.SaveChangesAsync();
                    TempData["PomyslnaRezerwacja"] = "Pomyślnie dokonano rezerwacji";
                    return RedirectToAction("Szczegoly", "Hotele", new { id = model.HotelId });
                }
                else
                {
                    // Jeśli termin jest zajęty, przekieruj do widoku "Hotele/Szczegoly" z informacją
                    TempData["BladRezerwacji"] = "Termin jest zajęty. Wybierz inny okres.";
                    return RedirectToAction("Szczegoly", "Hotele", new { id = model.HotelId });
                }
        }

        public async Task<IActionResult> MojeRezerwacje()
        {
            // Pobierz aktualnie zalogowanego użytkownika
            // var user = await _userManager.GetUserAsync(User);

            // Pobierz rezerwacje użytkownika wraz z informacjami o hotelach i pokojach
            var mojeRezerwacje = await _context.Rezerwacja
                .Include(r => r.Pokoj)
                    .ThenInclude(p => p.Hotel) // Dodaj to, aby załączyć informacje o hotelu
                .Where(r => r.UzytkownikId == HttpContext.Session.GetString("UserId"))
                .ToListAsync();

            return View(mojeRezerwacje);
        }

        private async Task<bool> SprawdzDostepnoscPokoju(int pokojId, DateTime dataRozpoczecia, DateTime dataZakonczenia)
        {
            // Sprawdź, czy w danym okresie istnieją już rezerwacje dla danego pokoju
            var zajeteTerminy = await _context.Rezerwacja
                .Where(r => r.PokojId == pokojId)
                .Where(r => (dataRozpoczecia >= r.DataPoczatek && dataRozpoczecia <= r.DataKoniec) ||
                            (dataZakonczenia >= r.DataPoczatek && dataZakonczenia <= r.DataKoniec))
                .ToListAsync();

            return zajeteTerminy.Count == 0;
        }
    }
}
