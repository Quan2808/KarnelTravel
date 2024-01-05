using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.Migrations;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeedbackController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public FeedbackController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("/admin/feedback")]
        public async Task<IActionResult> Index()
        {
            var feedbacks = await _dbContext.Feedbacks
                                            .OrderByDescending(f => f.CommentDate) 
                                            .ToListAsync();
            return View(feedbacks);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbContext.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await _dbContext.Feedbacks
                .FirstOrDefaultAsync(m => m.ID == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }
    }
}
