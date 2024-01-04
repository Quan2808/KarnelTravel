using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;


        public RestaurantController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var restaurants = await _context.Restaurants.ToListAsync();

            var resData = restaurants.Select(restaurant => new
            {
                Restaurant = restaurant,
                NumRatings = _context.Ratings.Count(r => r.Booking.RestaurantID == restaurant.ID),
                TotalRatingValue = _context.Ratings
                                    .Where(r => r.Booking.RestaurantID == restaurant.ID)
                                    .Sum(r => r.Value)
            });

            if (!String.IsNullOrEmpty(search))
            {
                resData = resData.Where(data => data.Restaurant.Location!.Contains(search)).ToList();
            }

            return View(resData);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || _context.Restaurants == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(m => m.ID == id);

            if (restaurant == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var resData = new
            {
                Restaurant = restaurant,
                Review = _context.Ratings.Where(r => r.Booking!.RestaurantID == restaurant.ID).ToList(),
                NumRatings = _context.Ratings.Count(r => r.Booking!.RestaurantID == restaurant.ID),
                TotalRatingValue = _context.Ratings
                                   .Where(r => r.Booking!.RestaurantID == restaurant.ID)
                                   .Sum(r => r.Value)
            };

            return View(resData);
        }
    }
}
