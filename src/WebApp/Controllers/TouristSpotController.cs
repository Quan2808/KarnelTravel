using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Immutable;
using System.Diagnostics;
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
                tourist = tourist.Where(l => l.Location
                    .Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
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
                return RedirectToAction(nameof(Index));
            }

            var tourists = await _context.Tourists.FirstOrDefaultAsync(m => m.ID == id);

            var travel = _context.Travels.Where(tr => tr.TouristSpotID == tourists!.ID).ToList();

            var travelID = await _context.Travels
                .Where(tr => tr.TouristSpotID == tourists.ID)
                .FirstOrDefaultAsync();

            if (tourists == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var travelData = new
            {
                Travel = travel,
                Tourists = tourists,
            };

            return View(travelData);
        }
    }
}
