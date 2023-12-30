using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using WebApp.Data;

namespace WebApp.Areas.Admin
{
    [Area("Admin")]
    public class TravelController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public TravelController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Travels.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.TouristSpotID = new SelectList(_context.Tourists.ToList(), "ID", "Name");

            if (id == null || _context.Travels == null)
            {
                return NotFound();
            }

            var travelInfo = await _context.Travels
                .FirstOrDefaultAsync(m => m.ID == id);
            if (travelInfo == null)
            {
                return NotFound();
            }

            return View(travelInfo);
        }

        public IActionResult Create()
        {
            ViewBag.TouristSpotID = new SelectList(_context.Tourists.ToList(), "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,TouristSpotID,StartingTime,EndingTime")] TravelInfo travelInfo)
        {
            try
            {
                ViewBag.TouristSpotID = new SelectList(_context.Tourists.ToList(), "ID", "Name", travelInfo.TouristSpotID);

                if (ModelState.IsValid)
                {
                    _context.Add(travelInfo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                return View(travelInfo);
            }
            catch
            {
                return View(travelInfo);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.TouristSpotID = new SelectList(_context.Tourists.ToList(), "ID", "Name");

            var travelInfo = await _context.Travels.FindAsync(id);
            if (travelInfo == null)
            {
                return NotFound();
            }
            return View(travelInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,TouristSpotID,StartingTime,EndingTime")] TravelInfo travelInfo)
        {
            ViewBag.TouristSpotID = new SelectList(_context.Tourists.ToList(), "ID", "Name", travelInfo.TouristSpotID);

            var existingTravel = await _context.Travels.FindAsync(id);

            try
            {
                existingTravel.Name = travelInfo.Name;
                existingTravel.Price = travelInfo.Price;
                existingTravel.Description = travelInfo.Description;
                existingTravel.TouristSpotID = travelInfo.TouristSpotID;
                existingTravel.StartingTime = travelInfo.StartingTime;
                existingTravel.EndingTime = travelInfo.EndingTime;

                _context.Update(existingTravel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(existingTravel);
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existingTravel = await _context.Travels.FindAsync(id);

            if (existingTravel == null)
            {
                return NotFound();
            }

            _context.Travels.Remove(existingTravel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TravelExists(int id)
        {
            return (_context.Travels?.Any(e => e.ID == id)).GetValueOrDefault();
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
