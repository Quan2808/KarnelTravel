﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;


        public RestaurantController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Restaurants.ToListAsync());
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || _context.Restaurants == null)
            {
                return NotFound();
            }

            var hotel = await _context.Restaurants
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }
    }
}