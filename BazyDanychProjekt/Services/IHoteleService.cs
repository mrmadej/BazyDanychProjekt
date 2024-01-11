using BazyDanychProjekt.Models;
using System.Collections.Generic;

namespace BazyDanychProjekt.Services
{
    public interface IHoteleService
    {
        List<Hotel> PobierzHotele();
        Hotel PobierzHotel(int id);
        string GetFirstHotelImage(int hotelId);
    }
}
