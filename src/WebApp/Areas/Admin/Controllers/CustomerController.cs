using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> IndexAsync()
        {
            var u = await (from user in _userManager.Users
                            join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                            join role in _roleManager.Roles on userRole.RoleId equals role.Id
                            select new 
                            {
                                Email = user.Email,
                                Phone = user.PhoneNumber.ToString(),
                                FullName = user.FirstName + " " + user.LastName,
                                Role = role.Name
                            }).ToListAsync();

            return View(u);
        }
    }
}
