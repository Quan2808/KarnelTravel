﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;


        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var hotels = await _context.Hotels.ToListAsync();

            if (!String.IsNullOrEmpty(search))
            {
                hotels = hotels.Where(l=>l.Location!.Contains(search)).ToList();
            }

            return View(hotels);
        }

        public async Task<IActionResult> Detail(int? id)
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
    }
}
