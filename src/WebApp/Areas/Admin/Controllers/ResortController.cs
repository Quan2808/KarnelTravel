using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using WebApp.Data;

namespace WebApp.Areas.Admin
{
    [Area("Admin")]
    public class ResortController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ResortController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string? search, int pg = 1)
        {
            List<Resort> resorts = await _context.Resorts.ToListAsync();
            int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = resorts.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = resorts.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;

            if (!String.IsNullOrEmpty(search))
            {
                data = _context.Resorts.Where(p => p.Name.Contains(search)).ToList();
            }

            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Resorts == null)
            {
                return NotFound();
            }

            var Resort = await _context.Resorts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (Resort == null)
            {
                return NotFound();
            }

            return View(Resort);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Price,Description")] Resort Resort, IFormFile imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var imagePath = await SaveImageAsync(imageFile, Resort);
                        Resort.Image = imagePath;
                    }

                    _context.Add(Resort);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View(Resort);
            }
            return View(Resort);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var Resort = await _context.Resorts.FindAsync(id);
            if (Resort == null)
            {
                return NotFound();
            }
            return View(Resort);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Price,Description")] Resort Resort, IFormFile imageFile)
        {
            var existingResort = await _context.Resorts.FindAsync(id);

            try
            {

                if (imageFile != null && imageFile.Length > 0)
                {
                    var imagePath = await SaveImageAsync(imageFile, Resort);
                    existingResort.Image = imagePath;
                }
                else
                {
                    existingResort.Image = existingResort.Image;
                }

                existingResort.Name = Resort.Name;
                existingResort.Location = Resort.Location;
                existingResort.Price = Resort.Price;
                existingResort.Description = Resort.Description;

                _context.Update(existingResort);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(existingResort);
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existingResort = await _context.Resorts.FindAsync(id);

            if (existingResort == null)
            {
                return NotFound();
            }

            var imagePath = existingResort.Image.Substring(1);

            if (!string.IsNullOrEmpty(imagePath))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            var bookings = await _context.Bookings.Where(b => b.ResortID == id).ToListAsync();

            var tours = await _context.Tourists.Where(t => t.ResortID == id).ToListAsync();

            foreach (var booking in bookings)
            {
                _context.Bookings.Remove(booking);
                _context.Tourists.RemoveRange(tours);
            }

            _context.Resorts.Remove(existingResort);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ResortExists(int id)
        {
            return (_context.Resorts?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        public IActionResult Discard()
        {
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile, Resort Resort)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images/thumbails", "Resort", Resort.Name);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"Resort-{Guid.NewGuid().ToString()}.jpg";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
                stream.Close();
            }

            return $"/assets/images/thumbails/Resort/{Resort.Name}/{uniqueFileName}";
        }
    }
}
