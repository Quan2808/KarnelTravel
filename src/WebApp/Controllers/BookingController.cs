using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using SQLitePCL;
using WebApp.Data;

namespace WebApp.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var isUser = await _userManager.GetUserAsync(User);

            var booking = _context.Bookings
                .Include(b => b.Hotel)
                .Include(b => b.Resort)
                .Include(b => b.Restaurant)
                .Include(b => b.TouristSpot)
                .Include(b => b.TravelInfo)
                .Where(user => user.CustomerPhone == isUser.PhoneNumber);

            return View(await booking.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.GetUserAsync(User);
            var booking = await _context.Bookings
                .Include(b => b.Hotel)
                .Include(b => b.Resort)
                .Include(b => b.Restaurant)
                .Include(b => b.TouristSpot)
                .Include(b => b.TravelInfo)
                .FirstOrDefaultAsync(m => m.ID == id && m.CustomerPhone == user.PhoneNumber);
            if (booking == null)
            {
                return RedirectToAction("Index");
            }

            return View(booking);
        }

        public async Task<IActionResult> CheckIn(string package, int id)
        {
            if (package == null || id == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                ViewData["Package"] = package;
                ViewData["Id"] = id;
                var booking = new Booking
                {
                    CustomerName = user.FirstName + " " + user.LastName,
                    CustomerPhone = user.PhoneNumber,
                    CheckIn = DateTime.Today,
                    CheckOut = DateTime.Today,
                    Status = BookingStatus.Processing
                };

                switch (package)
                {
                    case "Hotel" when id > 0:
                        var hotel = _context.Hotels.FirstOrDefault(h => h.ID == id);
                        if (hotel != null)
                        {
                            booking.HotelID = id;
                            booking.TotalPrice = hotel.Price ?? 0;

                            ViewData["HotelName"] = hotel.Name;
                            ViewData["HotelPrice"] = hotel.Price;
                            ViewData["HotelLocation"] = hotel.Location;
                            ViewData["HotelDescription"] = hotel.Description;
                            ViewData["HotelImage"] = hotel.Image;
                        }
                        break;

                    case "Resort" when id > 0:
                        var resort = _context.Resorts.FirstOrDefault(r => r.ID == id);
                        if (resort != null)
                        {
                            booking.ResortID = id;
                            booking.TotalPrice = resort.Price ?? 0;

                            ViewData["ResortName"] = resort.Name;
                            ViewData["ResortPrice"] = resort.Price;
                            ViewData["ResortLocation"] = resort.Location;
                            ViewData["ResortDescription"] = resort.Description;
                            ViewData["ResortImage"] = resort.Image;
                        }
                        break;

                    case "Restaurant" when id > 0:
                        var restaurant = _context.Restaurants.FirstOrDefault(r => r.ID == id);
                        if (restaurant != null)
                        {
                            booking.RestaurantID = id;
                            booking.TotalPrice = restaurant.Price ?? 0;

                            ViewData["RestaurantName"] = restaurant.Name;
                            ViewData["RestaurantPrice"] = restaurant.Price;
                            ViewData["RestaurantLocation"] = restaurant.Location;
                            ViewData["RestaurantDescription"] = restaurant.Description;
                            ViewData["RestaurantImage"] = restaurant.Image;
                        }
                        break;

                    case "TouristSpot" when id > 0:
                        booking.TouristSpotID = id;
                        break;

                    case "TravelInfo" when id > 0:
                        booking.TravelInfoID = id;
                        booking.TotalPrice = _context.Travels.FirstOrDefault(t => t.ID == id)?.Price ?? 0;
                        break;

                    default:
                        return View(booking);
                }

                return View(booking);
            }

            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn([Bind("HotelID,ResortID,RestaurantID,TouristSpotID,TravelInfoID,CustomerName,CustomerPhone,CheckIn,CheckOut,Status")] Booking booking, string package, int id)
        {
            if (ModelState.IsValid)
            {
                int numberOfDays = (int)(booking.CheckOut - booking.CheckIn).TotalDays;
                switch (package)
                {
                    case "Hotel" when id > 0:
                        booking.HotelID = id;
                        booking.TotalPrice = numberOfDays * _context.Hotels.FirstOrDefault(h => h.ID == id)?.Price ?? 0;
                        break;

                    case "Resort" when id > 0:
                        booking.ResortID = id;
                        booking.TotalPrice = numberOfDays * _context.Resorts.FirstOrDefault(r => r.ID == id)?.Price ?? 0;
                        break;

                    case "Restaurant" when id > 0:
                        booking.RestaurantID = id;
                        booking.TotalPrice = numberOfDays * _context.Restaurants.FirstOrDefault(r => r.ID == id)?.Price ?? 0;
                        break;

                    case "TouristSpot" when id > 0:
                        booking.TouristSpotID = id;
                        break;

                    case "TravelInfo" when id > 0:
                        booking.TravelInfoID = id;
                        booking.TotalPrice = numberOfDays * _context.Travels.FirstOrDefault(t => t.ID == id)?.Price ?? 0;
                        break;

                    default:
                        return RedirectToAction("Index");
                }
                booking.Status = BookingStatus.Processing;
                if (booking.CheckIn >= DateTime.Today && booking.CheckOut > booking.CheckIn)
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("CheckIn", "Invalid CheckIn or CheckOut date");
                }
            }

            ViewData["Package"] = package;
            ViewData["Id"] = id;

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelled(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                booking.Status = BookingStatus.Cancelled;
                _context.Bookings.Update(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
