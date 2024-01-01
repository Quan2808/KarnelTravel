using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using WebApp.Data;

namespace WebApp.Areas.Admin
{
    [Area("Admin")]
    public class TouristSpotController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public TouristSpotController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Tourists.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
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

        public IActionResult Create()
        {
            ViewBag.HotelID = new SelectList(_context.Hotels.ToList(), "ID", "Name");
            ViewBag.ResortID = new SelectList(_context.Resorts.ToList(), "ID", "Name");
            ViewBag.RestaurantID = new SelectList(_context.Restaurants.ToList(), "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Description,HotelID,ResortID,RestaurantID")] TouristSpot touristSpot)
        {
            try
            {
                ViewBag.HotelID = new SelectList(_context.Hotels.ToList(), "ID", "Name", touristSpot.HotelID);
                ViewBag.ResortID = new SelectList(_context.Resorts.ToList(), "ID", "Name", touristSpot.ResortID);
                ViewBag.RestaurantID = new SelectList(_context.Restaurants.ToList(), "ID", "Name", touristSpot.RestaurantID);

                if (ModelState.IsValid)
                {
                    _context.Add(touristSpot);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(touristSpot);
            }
            catch
            {
                return View(touristSpot);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.HotelID = new SelectList(_context.Hotels.ToList(), "ID", "Name");
            ViewBag.ResortID = new SelectList(_context.Resorts.ToList(), "ID", "Name");
            ViewBag.RestaurantID = new SelectList(_context.Restaurants.ToList(), "ID", "Name");

            var touristSpot = await _context.Tourists.FindAsync(id);
            if (touristSpot == null)
            {
                return NotFound();
            }
            return View(touristSpot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Description,HotelID,ResortID,RestaurantID")] TouristSpot touristSpot)
        {
            ViewBag.HotelID = new SelectList(_context.Hotels.ToList(), "ID", "Name", touristSpot.HotelID);
            ViewBag.ResortID = new SelectList(_context.Resorts.ToList(), "ID", "Name", touristSpot.ResortID);
            ViewBag.RestaurantID = new SelectList(_context.Restaurants.ToList(), "ID", "Name", touristSpot.RestaurantID);

            var exitingTourist = await _context.Tourists.FindAsync(id);

            try
            {
                exitingTourist.Name = touristSpot.Name;
                exitingTourist.Location = touristSpot.Location;
                exitingTourist.Description = touristSpot.Description;
                exitingTourist.HotelID = touristSpot.HotelID;
                exitingTourist.ResortID = touristSpot.ResortID;
                exitingTourist.RestaurantID = touristSpot.RestaurantID;

                _context.Update(exitingTourist);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(exitingTourist);
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exitingTourist = await _context.Tourists.FindAsync(id);

            if (exitingTourist == null)
            {
                return NotFound();
            }

            var bookings = await _context.Bookings.Where(b => b.TouristSpotID == id).ToListAsync();

            foreach (var booking in bookings)
            {
                _context.Bookings.Remove(booking);
            }

            _context.Tourists.Remove(exitingTourist);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TouristExists(int id)
        {
            return (_context.Tourists?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        public IActionResult Discard()
        {
            return RedirectToAction(nameof(Index));
        }

        //private async Task<string> SaveImageAsync(IFormFile imageFile, Hotel hotel)
        //{
        //    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images/thumbails", "Hotel", hotel.Name);

        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        Directory.CreateDirectory(uploadsFolder);
        //    }

        //    var uniqueFileName = $"Hotel-{Guid.NewGuid().ToString()}.jpg";
        //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await imageFile.CopyToAsync(stream);
        //        stream.Close();
        //    }

        //    return $"/assets/images/thumbails/Hotel/{hotel.Name}/{uniqueFileName}";
        //}
    }
}