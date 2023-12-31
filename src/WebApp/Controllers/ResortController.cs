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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Resorts.ToListAsync());
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
