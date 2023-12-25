using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HotelController : Controller
    {
        private readonly TestKT001Context db;
        IWebHostEnvironment host;
        public HotelController(TestKT001Context db, IWebHostEnvironment host)
        {
            this.db = db;
            this.host = host;
        }
        public IActionResult Index()
        {
            var model = db.Hotels.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {   
            return View();
        }

        [HttpPost]
        public IActionResult Create(HotelViewModel hotel1)
        {
            string fileName = "";
            if (hotel1.Photo!=null)
            {
                string uploadFolder = Path.Combine(host.WebRootPath, "imagesHotel");
                fileName = Guid.NewGuid().ToString() + "_" + hotel1.Photo.FileName;
                string filePath = Path.Combine(uploadFolder, fileName);
                //hotel1.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    hotel1.Photo.CopyTo(stream);
                    stream.Close();
                }
            }
            Hotel h = new Hotel
            {
                Id = hotel1.Id,
                Name = hotel1.Name,
                Description = hotel1.Description,
                Price = hotel1.Price,
                Location = hotel1.Location,
                Image = fileName
            };
            db.Hotels!.Add(h);
            db.SaveChanges();
            TempData["AlertCreate"] = "Hotel Added";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id) 
        {
            if (id == 0)
            {
                return NotFound();
            }
            var data  = db.Hotels.Where(h => h.Id == id).SingleOrDefault();
            return View(data);
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var data = db.Hotels.Where(h => h.Id == id).FirstOrDefault();
            return View(data);
        }

        [HttpPost]
        public IActionResult Update(int? id, Hotel hotel, IFormFile file)
        {
           if(id == null)
           {
                return NotFound();     
           }
           var h = db.Hotels.Where(x=>x.Id == id).FirstOrDefault();
           if (h == null)
           {
                return NotFound();
           }

           h.Name = hotel.Name;
           h.Description = hotel.Description;
           h.Price = hotel.Price;
           h.Location = hotel.Location;
           if (file != null)
           {
                string deleteFromFolder =Path.Combine(host.WebRootPath, "imagesHotel");
                string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, h.Image!);
                if (currentImage != null)
                {
                    if (System.IO.File.Exists(currentImage))
                    {
                        System.IO.File.Delete(currentImage);
                    }
                }

                string filename = Guid.NewGuid().ToString() + ".jpg";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagesHotel", filename);
                using (var stream = new FileStream(path, FileMode.Create))
                { 
                    file.CopyTo(stream);
                    stream.Close();
                }
                h.Image = filename;
            }
           db.SaveChanges();
           TempData["AlertUpdate"] = "Hotel Updated";
           return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            else
            {
                var data = db.Hotels.Where(h => h.Id == id).SingleOrDefault();
                if (data != null)
                {
                    string deleteFromFolder = Path.Combine(host.WebRootPath, "imagesHotel");
                    string currentImage = Path.Combine(Directory.GetCurrentDirectory(), deleteFromFolder, data.Image!);
                    if (currentImage != null)
                    {
                        if (System.IO.File.Exists(currentImage))
                        {
                            System.IO.File.Delete(currentImage);
                        }
                    }
                    db.Hotels.Remove(data);
                    db.SaveChanges();
                    TempData["AlertDelete"] = "Deleted Hotel";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
