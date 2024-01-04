using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Model;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            // Order hotels by ID in descending order and take the top 10
            var hotels = await _context.Hotels
                .OrderByDescending(h => h.ID)
                .Take(4)
                .ToListAsync();

            var hotelData = hotels.Select(hotel => new
            {
                Hotel = hotel,
                NumRatings = _context.Ratings.Count(r => r.Booking.HotelID == hotel.ID),
                TotalRatingValue = _context.Ratings
                                    .Where(r => r.Booking.HotelID == hotel.ID)
                                    .Sum(r => r.Value)
            });

            if (!String.IsNullOrEmpty(search))
            {
                hotelData = hotelData.Where(data => data.Hotel.Location!.Contains(search)).ToList();
            }

            return View(hotelData);
        }

        public IActionResult Introduction()
        {
            return View();
        }
    }
}
