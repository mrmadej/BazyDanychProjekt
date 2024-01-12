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

            var hotel = await _context.Hotele.FindAsync(id);
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

            if (ModelState.IsValid)
            {
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
            }
            return View(hotel);
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
        public async Task<IActionResult> PotwierdzUsuniecieHotelu(int id)
        {
            var hotel = await _context.Hotele.FindAsync(id);
            _context.Hotele.Remove(hotel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
            return _context.Hotele.Any(e => e.Id == id);
        }
    }
}
