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

        public async Task<IActionResult> Index(string? search)
        {

            var travels = await _context.Travels.ToListAsync();

            if (!String.IsNullOrEmpty(search))
            {
                travels = _context.Travels.Where(l => l.TouristSpot!.Location.Contains(search)).ToList();
            }

            ViewBag.TouristSpotID = new SelectList(_context.Tourists.ToList(), "ID", "Name");

            ViewBag.HotelID = new SelectList(_context.Hotels.ToList(), "ID", "Name");
            ViewBag.ResortID = new SelectList(_context.Resorts.ToList(), "ID", "Name");
            ViewBag.RestaurantID = new SelectList(_context.Restaurants.ToList(), "ID", "Name");

            return View(travels);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            ViewBag.TouristSpotID = new SelectList(_context.Tourists.ToList(), "ID", "Name");

            ViewBag.HotelID = new SelectList(_context.Hotels.ToList(), "ID", "Name");
            ViewBag.ResortID = new SelectList(_context.Resorts.ToList(), "ID", "Name");
            ViewBag.RestaurantID = new SelectList(_context.Restaurants.ToList(), "ID", "Name");

            if (id == null || _context.Travels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Travels
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }
    }
}
