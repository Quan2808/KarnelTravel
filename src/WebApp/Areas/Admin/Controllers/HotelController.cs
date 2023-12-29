using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using WebApp.Data;

namespace WebApp.Areas.Admin
{
    [Area("Admin")]
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public HotelController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Hotels.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Price,Description")] Hotel hotel, IFormFile imageFile)
        {
            System.Diagnostics.Debug.WriteLine("isValid:  " + ModelState.IsValid);
            try
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var imagePath = await SaveImageAsync(imageFile, hotel);
                        hotel.Image = imagePath;
                    }

                    _context.Add(hotel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View(hotel);
            }
            return View(hotel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Price,Description")] Hotel hotel, IFormFile imageFile)
        {
            var existingHotel = await _context.Hotels.FindAsync(id);

            try
            {

                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePathDel = existingHotel.Image.Substring(1);

                    if (!string.IsNullOrEmpty(imagePathDel))
                    {
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePathDel);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    var imagePath = await SaveImageAsync(imageFile, hotel);
                    existingHotel.Image = imagePath;
                }
                else
                {
                    existingHotel.Image = existingHotel.Image;
                }

                existingHotel.Name = hotel.Name;
                existingHotel.Location = hotel.Location;
                existingHotel.Price = hotel.Price;
                existingHotel.Description = hotel.Description;

                _context.Update(existingHotel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(existingHotel);
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existingHotel = await _context.Hotels.FindAsync(id);

            if (existingHotel == null)
            {
                return NotFound();
            }

            var imagePath = existingHotel.Image.Substring(1);

            if (!string.IsNullOrEmpty(imagePath))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Hotels.Remove(existingHotel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
            return (_context.Hotels?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        public IActionResult Discard()
        {
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile, Hotel hotel)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images/thumbails", "Hotel", hotel.Name);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"Hotel-{Guid.NewGuid().ToString()}.jpg";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
                stream.Close();
            }

            return $"/assets/images/thumbails/Hotel/{hotel.Name}/{uniqueFileName}";
        }
    }
}
