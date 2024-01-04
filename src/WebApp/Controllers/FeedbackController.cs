using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FeedbackController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
       
        public async Task<IActionResult> Index(string package, int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                ViewData["Package"] = package;
                ViewData["Id"] = id;
                var feedback = new Feedback
                {
                    CustomerName = user.UserName,
                    CustomerPhone = user.Email,
                    CommentDate = DateTime.Today,

                };

                return View(feedback);
            }

            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("ID,CustomerName,CustomerPhone,CommentDate,Comment")] Feedback feedback)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    feedback.CommentDate = DateTime.Now;

                    _context.Add(feedback);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Feedback submitted successfully.";

                    return RedirectToAction("Index", new { package = ViewData["Package"], id = ViewData["Id"] });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the feedback.");
            }

            return View(feedback);
        }
    }
}
