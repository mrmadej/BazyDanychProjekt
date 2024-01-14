using BazyDanychProjekt.Data;
using BazyDanychProjekt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazyDanychProjekt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminPanelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hotele = await _context.Hotele.ToListAsync();
            return View(hotele);
        }

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

            int dodanyHotelId = hotel.Id;

            var zdjecie = new Zdjecie
            {
                HotelId = dodanyHotelId,
                Url = model.url
            };
            _context.Zdjecia.Add(zdjecie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EdytujHotel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotele
                .Include(h => h.Zdjecia)
                .Include(h => h.Pokoje)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> EdytujHotel(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return NotFound();
            }

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

            var noweZdjecie = new Zdjecie { Url = url };
            hotel.Zdjecia.Add(noweZdjecie);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EdytujZdjeciaHotelu), new { id = hotelId });
        }

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

            ViewData["idHotelu"] = zdjecie.HotelId;

            return View(zdjecie);
        }

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

            return RedirectToAction(nameof(EdytujZdjeciaHotelu), new { id = idHotelu });
        }

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

        [HttpPost, ActionName("UsunHotel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotwierdzUsuniecieHotelu(int id)
        {
            var hotel = await _context.Hotele.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            hotel.Pokoje = await _context.Pokoj.Where(p => p.HotelId == id).ToListAsync();
            hotel.Opinie = await _context.Opinia.Where(o => o.HotelId == id).ToListAsync();

            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotwierdzUsuniecieHoteluPotwierdzenie(int id)
        {
            var hotel = await _context.Hotele.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

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
    }
}
