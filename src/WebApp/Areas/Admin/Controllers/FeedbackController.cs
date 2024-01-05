using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
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

        public async Task<IActionResult> Index(string? search, int pg = 1)
        {
            //var feedbacks = await _dbContext.Feedbacks
            //                                .OrderByDescending(f => f.CommentDate) 
            //                                .ToListAsync();

            //return View(feedbacks);

            List<Feedback> feedbacks = await _dbContext.Feedbacks
                                                .OrderByDescending(f => f.CommentDate)
                                                .ToListAsync();
            int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = feedbacks.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = feedbacks.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;

            if (!String.IsNullOrEmpty(search))
            {
                data = _dbContext.Feedbacks.Where(p => p.CustomerName.Contains(search)
                                                    || p.CustomerPhone.Contains(search)
                                                    || p.Comment.Contains(search))
                                            .ToList();
            }

            return View(data);
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
