using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Model;
using WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? search)
        {
            // Order hotels by ID in descending order and take the top 10
            var hotels = await _context.Hotels
                .OrderByDescending(h => h.ID)
                .Take(4)
                .ToListAsync();

            var hotelData = hotels.Select(hotel => new
            {
                Hotel = hotel,
                NumRatings = _context.Ratings.Count(r => r.Booking.HotelID == hotel.ID),
                TotalRatingValue = _context.Ratings
                                    .Where(r => r.Booking.HotelID == hotel.ID)
                                    .Sum(r => r.Value)
            });

            if (!String.IsNullOrEmpty(search))
            {
                hotelData = hotelData.Where(data => data.Hotel.Location!.Contains(search)).ToList();
            }

            return View(hotelData);
        }

        public IActionResult Introduction()
        {
            return View();
        }

        public async Task<IActionResult> FeedBack()
        {
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    var feedback = new Feedback
                    {
                        CustomerName = user.FirstName + " " + user.LastName,
                        CustomerPhone = user.Email,
                        CommentDate = DateTime.Today,
                    };
                    return View(feedback);
                }

                var feedbackNoneUser = new Feedback
                {
                    CommentDate = DateTime.Today,
                };

                return View(feedbackNoneUser);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FeedBack([Bind("ID,CustomerName,CustomerPhone,CommentDate,Comment")] Feedback feedback)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);

                    if (user != null)
                    {
                        feedback.CustomerName = user.FirstName + " " + user.LastName;
                        feedback.CustomerPhone = user.PhoneNumber;
                    }

                    feedback.CommentDate = DateTime.Now;

                    _context.Add(feedback);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Feedback submitted successfully.";

                    return RedirectToAction("feedback");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the feedback.");
            }

            return View(feedback);
        }
    }
}
