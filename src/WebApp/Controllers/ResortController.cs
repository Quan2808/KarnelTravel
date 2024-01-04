using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class ResortController : Controller
    {
        private readonly ApplicationDbContext _context;


        public ResortController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var resorts = await _context.Hotels.ToListAsync();

            var resortData = resorts.Select(resort => new
            {
                Resort = resort,
                NumRatings = _context.Ratings.Count(r => r.Booking.ResortID == resort.ID),
                TotalRatingValue = _context.Ratings
                                    .Where(r => r.Booking.ResortID == resort.ID)
                                    .Sum(r => r.Value)
            });

            if (!String.IsNullOrEmpty(search))
            {
                resortData = resortData.Where(data => data.Resort.Location!.Contains(search)).ToList();
            }

            return View(resortData);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || _context.Resorts == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var resort = await _context.Resorts.FirstOrDefaultAsync(m => m.ID == id);

            if (resort == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var resortData = new
            {
                Resort = resort,
                Review = _context.Ratings.Where(r => r.Booking!.ResortID == resort.ID).ToList(),
                NumRatings = _context.Ratings.Count(r => r.Booking!.ResortID == resort.ID),
                TotalRatingValue = _context.Ratings
                                   .Where(r => r.Booking!.ResortID == resort.ID)
                                   .Sum(r => r.Value)
            };

            return View(resortData);
        }
    }
}
