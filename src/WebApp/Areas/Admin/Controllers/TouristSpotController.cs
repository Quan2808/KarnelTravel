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

        public async Task<IActionResult> Index(string? search, int pg = 1)
        {
            List<TouristSpot> touristSpots = await _context.Tourists.OrderByDescending(t => t.ID).ToListAsync();
            int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = touristSpots.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = touristSpots.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;

            if (!String.IsNullOrEmpty(search))
            {
                data = _context.Tourists.Where(p => p.Name.Contains(search)).ToList();
            }

            return View(data);
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
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Location,Description,HotelID,ResortID,RestaurantID")]
            TouristSpot touristSpot, IFormFile imageFile)
        {
            try
            {
                ViewBag.HotelID = new SelectList
                    (_context.Hotels.ToList(), "ID", "Name", touristSpot.HotelID);
                ViewBag.ResortID = new SelectList
                    (_context.Resorts.ToList(), "ID", "Name", touristSpot.ResortID);
                ViewBag.RestaurantID = new SelectList
                    (_context.Restaurants.ToList(), "ID", "Name", touristSpot.RestaurantID);

                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var imagePath = await SaveImageAsync(imageFile, touristSpot);
                        touristSpot.Image = imagePath;
                    }
                    _context.Add(touristSpot);
                    await _context.SaveChangesAsync();
                    TempData["AlertCreate"] = "Tourist Created Successfuly!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View(touristSpot);
            }
            return View(touristSpot);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.HotelID = new SelectList
                (_context.Hotels.ToList(), "ID", "Name");
            ViewBag.ResortID = new SelectList
                (_context.Resorts.ToList(), "ID", "Name");
            ViewBag.RestaurantID = new SelectList
                (_context.Restaurants.ToList(), "ID", "Name");

            var touristSpot = await _context.Tourists.FindAsync(id);
            if (touristSpot == null)
            {
                return NotFound();
            }
            return View(touristSpot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Description,HotelID,ResortID,RestaurantID")] TouristSpot touristSpot, IFormFile imageFile)
        {
            ViewBag.HotelID = new SelectList
                (_context.Hotels.ToList(), "ID", "Name", touristSpot.HotelID);
            ViewBag.ResortID = new SelectList
                (_context.Resorts.ToList(), "ID", "Name", touristSpot.ResortID);
            ViewBag.RestaurantID = new SelectList
                (_context.Restaurants.ToList(), "ID", "Name", touristSpot.RestaurantID);

            var exitingTourist = await _context.Tourists.FindAsync(id);

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePathDel = exitingTourist.Image.Substring(1);
                    if (!string.IsNullOrEmpty(imagePathDel))
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePathDel);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    var imagePath = await SaveImageAsync(imageFile, touristSpot);
                    exitingTourist.Image = imagePath;
                }
                else
                {
                    exitingTourist.Image = exitingTourist.Image;
                }

                exitingTourist.Name = touristSpot.Name;
                exitingTourist.Location = touristSpot.Location;
                exitingTourist.Description = touristSpot.Description;
                exitingTourist.HotelID = touristSpot.HotelID;
                exitingTourist.ResortID = touristSpot.ResortID;
                exitingTourist.RestaurantID = touristSpot.RestaurantID;

                _context.Update(exitingTourist);
                await _context.SaveChangesAsync();
                TempData["AlertEdit"] = "Tourist Saved Successfuly!";
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

            var imagePath = exitingTourist.Image.Substring(1);

            if (!string.IsNullOrEmpty(imagePath))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            var bookings = await _context.Bookings
                .Where(b => b.TravelInfo.TouristSpotID == id).ToListAsync();

            var travels = await _context.Travels
                .Where(b => b.TouristSpotID == id).ToListAsync();

            foreach (var booking in bookings)
            {
                _context.Bookings.RemoveRange(booking);
            }

            _context.Tourists.Remove(exitingTourist);
            await _context.SaveChangesAsync();
            TempData["AlertDelete"] = "Tourist Deleted Successfuly!";
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

        private async Task<string> SaveImageAsync(IFormFile imageFile, TouristSpot touristSpot)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images/thumbails", "Tourist Spot", touristSpot.Name);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"Tourist-Spot-{Guid.NewGuid().ToString()}.jpg";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
                stream.Close();
            }

            return $"/assets/images/thumbails/Tourist Spot/{touristSpot.Name}/{uniqueFileName}";
        }
    }
}