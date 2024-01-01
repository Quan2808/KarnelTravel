using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class TouristSpotController : Controller
    {
        private readonly ApplicationDbContext _context;


        public TouristSpotController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var tourist = await _context.Tourists.ToListAsync();

            if (!String.IsNullOrEmpty(search))
            {
                tourist = tourist.Where(l => l.Location.Contains(search)).ToList();
            }

            ViewBag.HotelID = new SelectList(_context.Hotels.ToList(), "ID", "Name");
            ViewBag.ResortID = new SelectList(_context.Resorts.ToList(), "ID", "Name");
            ViewBag.RestaurantID = new SelectList(_context.Restaurants.ToList(), "ID", "Name");

            return View(tourist);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            ViewBag.HotelID = new SelectList(_context.Hotels.ToList(), "ID", "Name");
            ViewBag.ResortID = new SelectList(_context.Resorts.ToList(), "ID", "Name");
            ViewBag.RestaurantID = new SelectList(_context.Restaurants.ToList(), "ID", "Name");

            if (id == null || _context.Tourists == null)
            {
                return NotFound();
            }

            var hotel = await _context.Tourists
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }
    }
}
