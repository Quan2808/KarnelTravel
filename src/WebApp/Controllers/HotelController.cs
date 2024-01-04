using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;


        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int? rating, string? sortByPrice)
        {
            var hotels = await _context.Hotels.ToListAsync();

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

            if (rating.HasValue)
            {
                hotelData = hotelData.Where(data => data.NumRatings > 0 && data.TotalRatingValue / data.NumRatings == rating.Value).ToList();
            }
            if (!String.IsNullOrEmpty(sortByPrice))
            {
                if (sortByPrice.ToLower() == "asc")
                {
                    hotelData = hotelData.OrderBy(data => data.Hotel.Price).ToList();
                }
                else if (sortByPrice.ToLower() == "desc")
                {
                    hotelData = hotelData.OrderByDescending(data => data.Hotel.Price).ToList();
                }
            }
            return View(hotelData);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var hotel = await _context.Hotels.FirstOrDefaultAsync(m => m.ID == id);

            if (hotel == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var hotelData = new
            {
                Hotel = hotel,
                Review = _context.Ratings.Where(r => r.Booking!.HotelID == hotel.ID).ToList(),
                NumRatings = _context.Ratings.Count(r => r.Booking!.HotelID == hotel.ID),
                TotalRatingValue = _context.Ratings
                                   .Where(r => r.Booking!.HotelID == hotel.ID)
                                   .Sum(r => r.Value)
            };

            return View(hotelData);
        }
    }
}
