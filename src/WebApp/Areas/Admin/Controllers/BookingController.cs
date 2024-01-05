using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using WebApp.Data;

namespace WebApp.Areas.Admin
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookingController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string? search,int pg = 1)
        {
            List<Booking> bookings = await _context.Bookings.ToListAsync();
            int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = bookings.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = bookings.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            if (!String.IsNullOrEmpty(search))
            {
                data = bookings.Where(p => p.CustomerName.Contains(search) 
                                        || p.CustomerPhone.Contains(search))
                                        .ToList();
            }

            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Hotel)
                .Include(b => b.Resort)
                .Include(b => b.Restaurant)
                .Include(b => b.TouristSpot)
                .Include(b => b.TravelInfo)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost, ActionName("Update")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, Booking booking)
        {
            var existingBooking = await _context.Bookings.FindAsync(id);
            try
            {
                existingBooking.Status = booking.Status;

                _context.Update(existingBooking);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(existingBooking);
            }
        }
    }
}
