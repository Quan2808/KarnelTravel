using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public CustomerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var users = await (from user in _userManager.Users
                               join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                               join role in _roleManager.Roles on userRole.RoleId equals role.Id
                               select new
                               {
                                   UserName = user.UserName,
                                   UserId = user.Id,
                                   Name = user.FirstName + " " + user.LastName,
                                   Email = user.Email,
                                   Phone = user.PhoneNumber,
                                   Role = role.Name
                               }).ToListAsync();
            var availableRoles = await _roleManager.Roles.ToListAsync();

            ViewBag.AvailableRoles = availableRoles;

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string selectedRole)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentRole = currentRoles.FirstOrDefault();

            await _userManager.RemoveFromRoleAsync(user, currentRole);

            await _userManager.AddToRoleAsync(user, selectedRole);

            return RedirectToAction("Index");
        }
    }
}