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
            var resorts = await _context.Resorts.ToListAsync();

            if (!String.IsNullOrEmpty(search))
            {
                resorts = resorts.Where(l => l.Location!.Contains(search)).ToList();
            }

            return View(resorts);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || _context.Resorts == null)
            {
                return NotFound();
            }

            var hotel = await _context.Resorts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }
    }
}
