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

        public async Task<IActionResult> Index(string? search, int? rating, string? sortByPrice)
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
                resData = resData.Where(data =>
                    data.Restaurant.Location!.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    data.Restaurant.Name!.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            if (rating.HasValue)
            {
                resData = resData.Where(data => data.NumRatings > 0 && data.TotalRatingValue / data.NumRatings == rating.Value).ToList();
            }
            if (!String.IsNullOrEmpty(sortByPrice))
            {
                if (sortByPrice.ToLower() == "asc")
                {
                    resData = resData.OrderBy(data => data.Restaurant.Price).ToList();
                }
                else if (sortByPrice.ToLower() == "desc")
                {
                    resData = resData.OrderByDescending(data => data.Restaurant.Price).ToList();
                }
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
