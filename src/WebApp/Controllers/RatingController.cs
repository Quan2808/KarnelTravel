using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RatingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ratings.Include(r => r.Booking);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var isRating = new Rating
                {
                    CustomerName = user.FirstName + " " + user.LastName,
                    CustomerPhone = user.PhoneNumber,
                    CreatedAt = DateTime.Today,
                };

                return View(isRating);
            }

            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ID,Value,Comment,CustomerName,CustomerPhone,BookingID")] Rating rating)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(r => r.ID == rating.BookingID);

            if (booking != null)
            {
                rating.CreatedAt = DateTime.Now;
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rating);
        }
    }
}
