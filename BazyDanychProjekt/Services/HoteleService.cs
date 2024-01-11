using BazyDanychProjekt.Data;
using BazyDanychProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BazyDanychProjekt.Services
{
    public class HoteleService : IHoteleService
    {
        private readonly ApplicationDbContext _context;

        public HoteleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Hotel> PobierzHotele()
        {
            // Pobierz listę hoteli z bazy danych
            return _context.Hotele.ToList();
        }

        public Hotel PobierzHotel(int id)
        {
            // Pobierz hotel o danym id z bazy danych
            return _context.Hotele
                     .Include(h => h.Zdjecia)
                     .Include(h => h.Pokoje)
                     .Include(h => h.Opinie)
                     .ThenInclude(o => o.Uzytkownik)
                     .FirstOrDefault(h => h.Id == id);
        }
        public string GetFirstHotelImage(int hotelId)
        {
            var hotel = PobierzHotel(hotelId);
            if (hotel != null && hotel.Zdjecia != null && hotel.Zdjecia.Any())
            {
                // Zwróć ścieżkę do pierwszego zdjęcia
                return hotel.Zdjecia.First().Url;
            }

            // Zwróć domyślną ścieżkę, jeśli brak zdjęcia
            return "C:\\Users\\marci\\source\\repos\\BazyDanychProjekt\\BazyDanychProjekt\\brakZdjecia.jpg";
        }
    }
}
