using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _dbContext.Feedbacks.ToListAsync();
            ViewBag.FeedbackCount = await _dbContext.Feedbacks.CountAsync();

            var tour = await _dbContext.Tourists.ToListAsync();
            ViewBag.TourCount = await _dbContext.Tourists.CountAsync();

            var hotel = await _dbContext.Hotels.ToListAsync();
            ViewBag.HotelCount = await _dbContext.Hotels.CountAsync();

            var restaurant = await _dbContext.Restaurants.ToListAsync();
            ViewBag.ResCount = await _dbContext.Restaurants.CountAsync();

            var resort = await _dbContext.Resorts.ToListAsync();
            ViewBag.ResortCount = await _dbContext.Resorts.CountAsync();

            var booking = await _dbContext.Bookings.ToListAsync();
            ViewBag.BookingCount = await _dbContext.Bookings.CountAsync();

            var bookingLastest = await _dbContext.Bookings
                .OrderByDescending(b => b.ID)
                .Take(3)
                .Select(b => new { b.ID, b.CustomerName, b.CustomerPhone, b.TotalPrice })
                .ToListAsync();

            ViewBag.BookingLastest = bookingLastest;
            return View();
        }

    }
}
