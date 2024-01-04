using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class TravelController : Controller
    {
        private readonly ApplicationDbContext _context;


        public TravelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int? rating, string? sortByPrice)
        {

            var travels = await _context.Travels.ToListAsync();

            var travelData = travels.Select(travel => new
            {
                Travel = travel,
                NumRatings = _context.Ratings.Count(r => r.Booking.TravelInfoID == travel.ID),
                TotalRatingValue = _context.Ratings
                                    .Where(r => r.Booking.TravelInfoID == travel.ID)
                                    .Sum(r => r.Value)
            });

            if (!String.IsNullOrEmpty(search))
            {
                travelData = travelData.Where(data => data.Travel.TouristSpot.Location!.Contains(search)).ToList();
            }
            if (rating.HasValue)
            {
                travelData = travelData.Where(data => data.NumRatings > 0 && data.TotalRatingValue / data.NumRatings == rating.Value).ToList();
            }
            if (!String.IsNullOrEmpty(sortByPrice))
            {
                if (sortByPrice.ToLower() == "asc")
                {
                    travelData = travelData.OrderBy(data => data.Travel.Price).ToList();
                }
                else if (sortByPrice.ToLower() == "desc")
                {
                    travelData = travelData.OrderByDescending(data => data.Travel.Price).ToList();
                }
            }
            ViewBag.TouristSpotID = new SelectList(_context.Tourists.ToList(), "ID", "Name");

            ViewBag.HotelID = new SelectList(_context.Hotels.ToList(), "ID", "Name");
            ViewBag.ResortID = new SelectList(_context.Resorts.ToList(), "ID", "Name");
            ViewBag.RestaurantID = new SelectList(_context.Restaurants.ToList(), "ID", "Name");

            return View(travelData);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            ViewBag.TouristSpotID = new SelectList(_context.Tourists.ToList(), "ID", "Name");

            ViewBag.HotelID = new SelectList(_context.Hotels.ToList(), "ID", "Name");
            ViewBag.ResortID = new SelectList(_context.Resorts.ToList(), "ID", "Name");
            ViewBag.RestaurantID = new SelectList(_context.Restaurants.ToList(), "ID", "Name");

            if (id == null || _context.Travels == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var travel = await _context.Travels.FirstOrDefaultAsync(m => m.ID == id);

            if (travel == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var travelData = new
            {
                Travel = travel,
                Review = _context.Ratings.Where(r => r.Booking!.TravelInfoID == travel.ID).ToList(),
                NumRatings = _context.Ratings.Count(r => r.Booking!.TravelInfoID == travel.ID),
                TotalRatingValue = _context.Ratings
                                   .Where(r => r.Booking!.TravelInfoID == travel.ID)
                                   .Sum(r => r.Value)
            };

            return View(travelData);
        }
    }
}
