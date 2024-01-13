using BazyDanychProjekt.Data;
using BazyDanychProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazyDanychProjekt.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminPanelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Wyświetlanie wszystkich hoteli
        public async Task<IActionResult> Index()
        {
            var hotele = await _context.Hotele.ToListAsync();
            return View(hotele);
        }

        // Dodawanie nowego hotelu - GET
        //public IActionResult DodajHotel()
        //{
        //    return View();
        //}

        // Dodawanie nowego hotelu - POST
        [HttpPost]
        public async Task<IActionResult> DodajHotel(DodajHotelViewModel model)
        {
            var hotel = new Hotel
            {
                Nazwa = model.Nazwa,
                Kraj = model.Kraj,
                Miasto = model.Miasto,
                Gwiazdki = model.Gwiazdki,
                Opis = model.Opis
            };
            _context.Hotele.Add(hotel);
            await _context.SaveChangesAsync();

            // Teraz hotel ma przypisany identyfikator po zapisaniu w bazie danych
            int dodanyHotelId = hotel.Id;

            // Tutaj możesz użyć dodanyHotelId do zapisania zdjęcia w bazie danych
            // Na przykład, jeśli masz model Hotel zawierający kolekcję zdjęć, możesz dodać nowe zdjęcie dla hotelu.

            // Przykładowa logika dodawania zdjęcia:
            var zdjecie = new Zdjecie
            {
                HotelId = dodanyHotelId,
                Url = model.url
            };
            _context.Zdjecia.Add(zdjecie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Edytowanie istniejącego hotelu - GET
        public async Task<IActionResult> EdytujHotel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotele
                .Include(h => h.Zdjecia) // Dodaj to, jeśli chcesz pobrać zdjęcia razem z hotelem
                .Include(h => h.Pokoje)  // Dodaj to, jeśli chcesz pobrać pokoje razem z hotelem
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // Edytowanie istniejącego hotelu - POST
        [HttpPost]
        public async Task<IActionResult> EdytujHotel(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Entry(hotel).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
        }

        public async Task<IActionResult> EdytujZdjeciaHotelu(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotele
                .Include(h => h.Zdjecia)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> EdytujZdjeciaHotelu(int hotelId, string url)
        {
            var hotel = await _context.Hotele.Include(h => h.Zdjecia).FirstOrDefaultAsync(h => h.Id == hotelId);
            if (hotel == null)
            {
                return NotFound();
            }

            // Dodaj nowe zdjęcie
            var noweZdjecie = new Zdjecie { Url = url };
            hotel.Zdjecia.Add(noweZdjecie);

            // Zapisz zmiany w bazie danych
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EdytujZdjeciaHotelu), new { id = hotelId });
        }

        // Usuwanie zdjęcia - GET
        public async Task<IActionResult> UsunZdjecie(int? zdjecieId)
        {
            if (zdjecieId == null)
            {
                return NotFound();
            }

            var zdjecie = await _context.Zdjecia.FindAsync(zdjecieId);
            if (zdjecie == null)
            {
                return NotFound();
            }

            // Przekazanie idHotelu do potwierdzenia usuwania
            ViewData["idHotelu"] = zdjecie.HotelId;

            return View(zdjecie);
        }

        // Potwierdzenie usunięcia zdjęcia - POST
        [HttpPost]
        public async Task<IActionResult> PotwierdzUsuniecieZdjecia(int zdjecieId, int idHotelu)
        {
            var zdjecie = await _context.Zdjecia.FindAsync(zdjecieId);
            if (zdjecie == null)
            {
                return NotFound();
            }

            _context.Zdjecia.Remove(zdjecie);
            await _context.SaveChangesAsync();

            // Przekierowanie do EdytujZdjeciaHotelu z przekazanym idHotelu
            return RedirectToAction(nameof(EdytujZdjeciaHotelu), new { id = idHotelu });
        }

        // Usuwanie hotelu - GET
        public async Task<IActionResult> UsunHotel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotele
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // Usuwanie hotelu - POST
        [HttpPost, ActionName("UsunHotel")]
        [ValidateAntiForgeryToken] // Dodaj atrybut zabezpieczający przed atakami CSRF
        public async Task<IActionResult> PotwierdzUsuniecieHotelu(int id)
        {
            var hotel = await _context.Hotele.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            // Przypisz do modelu inne powiązane encje przed wyświetleniem widoku potwierdzenia
            hotel.Pokoje = await _context.Pokoj.Where(p => p.HotelId == id).ToListAsync();
            hotel.Opinie = await _context.Opinia.Where(o => o.HotelId == id).ToListAsync();

            return View(hotel);
        }

        // Potwierdzenie usuwania hotelu - POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Dodaj atrybut zabezpieczający przed atakami CSRF
        public async Task<IActionResult> PotwierdzUsuniecieHoteluPotwierdzenie(int id)
        {
            var hotel = await _context.Hotele.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            // Usuń powiązane encje przed usunięciem hotelu
            var pokoje = await _context.Pokoj.Where(p => p.HotelId == id).ToListAsync();
            var opinie = await _context.Opinia.Where(o => o.HotelId == id).ToListAsync();

            _context.Pokoj.RemoveRange(pokoje);
            _context.Opinia.RemoveRange(opinie);

            _context.Hotele.Remove(hotel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
            return _context.Hotele.Any(e => e.Id == id);
        }

        public async Task<IActionResult> WyswietlPokoje(int hotelId)
        {
            var pokoje = await _context.Pokoj
                .Where(p => p.HotelId == hotelId)
                .Include(p => p.Hotel)
                .ToListAsync();
            ViewBag.HotelId = hotelId;
            return View(pokoje);
        }

        [HttpPost]
        public async Task<IActionResult> DodajPokoj(DodajPokojViewModel nowyPokoj)
        {

            // Tutaj możesz dodać logikę sprawdzającą unikalność numeru pokoju, etc.
            var pokoj = new Pokoj
            {
                Nazwa = nowyPokoj.Nazwa,
                TypPokoju = nowyPokoj.TypPokoju,
                NumerPokoju = nowyPokoj.NumerPokoju,
                LiczbaOsob = nowyPokoj.LiczbaOsob,
                Cena = nowyPokoj.Cena,
                HotelId = nowyPokoj.HotelId,
            };

            _context.Pokoj.Add(pokoj);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(WyswietlPokoje), new { hotelId = nowyPokoj.HotelId });

            // Jeśli ModelState.IsValid nie jest spełniony, wróć do widoku z błędami
        }

        // Usuwanie pokoju - GET
        public async Task<IActionResult> UsunPokoj(int? pokojId)
        {
            if (pokojId == null)
            {
                return NotFound();
            }

            var pokoj = await _context.Pokoj.FindAsync(pokojId);

            if (pokoj == null)
            {
                return NotFound();
            }

            return View(pokoj);
        }

        // Usuwanie pokoju - POST
        [HttpPost]
        public async Task<IActionResult> PotwierdzUsunieciePokoj(int pokojId)
        {
            var pokoj = await _context.Pokoj.FindAsync(pokojId);

            if (pokoj == null)
            {
                return NotFound();
            }

            _context.Pokoj.Remove(pokoj);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(WyswietlPokoje), new { hotelId = pokoj.HotelId });
        }


        // Edytowanie istniejącego pokoju - GET
        public async Task<IActionResult> EdytujPokoj(int? pokojId)
        {
            if (pokojId == null)
            {
                return NotFound();
            }

            var pokoj = await _context.Pokoj
                .Include(p => p.Hotel) // Dodaj Include, aby pobrać obiekt Hotel wraz z pokojem
                .FirstOrDefaultAsync(p => p.Id == pokojId);

            if (pokoj == null)
            {
                return NotFound();
            }

            // Teraz masz dostęp do obiektu Hotel bezpośrednio z obiektu Pokoj
            var hotelId = pokoj.HotelId;

            return View(pokoj);
        }

        // Edytowanie istniejącego pokoju - POST
        [HttpPost]
        public async Task<IActionResult> EdytujPokoj(Pokoj pokoj)
        {
            try
            {
                _context.Entry(pokoj).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            // Po zaktualizowaniu pokoju, przenieś użytkownika do listy pokoi danego hotelu
            return RedirectToAction("WyswietlPokoje", new { hotelId = pokoj.HotelId });
        }

    }
}
