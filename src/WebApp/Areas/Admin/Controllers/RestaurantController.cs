using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using WebApp.Data;

namespace WebApp.Areas.Admin
{
    [Area("Admin")]
    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public RestaurantController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Restaurants.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Restaurants == null)
            {
                return NotFound();
            }

            var Restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Restaurant == null)
            {
                return NotFound();
            }

            return View(Restaurant);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Price,Description")] Restaurant Restaurant, IFormFile imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var imagePath = await SaveImageAsync(imageFile, Restaurant);
                        Restaurant.Image = imagePath;
                    }

                    _context.Add(Restaurant);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View(Restaurant);
            }
            return View(Restaurant);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var Restaurant = await _context.Restaurants.FindAsync(id);
            if (Restaurant == null)
            {
                return NotFound();
            }
            return View(Restaurant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Price,Description")] Restaurant Restaurant, IFormFile imageFile)
        {
            var existingRestaurant = await _context.Restaurants.FindAsync(id);

            try
            {

                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePath = await SaveImageAsync(imageFile, Restaurant);
                    existingRestaurant.Image = imagePath;
                }
                else
                {
                    existingRestaurant.Image = existingRestaurant.Image;
                }

                existingRestaurant.Name = Restaurant.Name;
                existingRestaurant.Location = Restaurant.Location;
                existingRestaurant.Price = Restaurant.Price;
                existingRestaurant.Description = Restaurant.Description;

                _context.Update(existingRestaurant);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(existingRestaurant);
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existingRestaurant = await _context.Restaurants.FindAsync(id);

            if (existingRestaurant == null)
            {
                return NotFound();
            }

            var imagePath = existingRestaurant.Image.Substring(1);

            if (!string.IsNullOrEmpty(imagePath))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Restaurants.Remove(existingRestaurant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return (_context.Restaurants?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        public IActionResult Discard()
        {
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile, Restaurant Restaurant)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images/thumbails", "Restaurant", Restaurant.Name);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"Restaurant-{Guid.NewGuid().ToString()}.jpg";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
                stream.Close();
            }

            return $"/assets/images/thumbails/Restaurant/{Restaurant.Name}/{uniqueFileName}";
        }
    }
}
