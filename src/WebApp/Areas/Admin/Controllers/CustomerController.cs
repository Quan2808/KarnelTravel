using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
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

        public async Task<IActionResult> Index(string? sortRole, string? search, int pg=1)
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
                                   Role = role.Name,
                                   Locked = user.LockoutEnabled
                               }).ToListAsync();
            var availableRoles = await _roleManager.Roles.ToListAsync();

            ViewBag.AvailableRoles = availableRoles;

            int pageSize = 10;
            if (pg < 1) pg = 1;
            int recsCount = users.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = users.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;

            if (!String.IsNullOrEmpty(search))
            {
                data = users.Where(p => p.Name.Contains(search) 
                                    || p.Email.Contains(search) 
                                    || p.Phone.Contains(search) 
                                    || p.Role.Contains(search))
                                        .ToList();
            }

            if (sortRole == "Admin" || sortRole == "User")
            {
                data = users.Where(p => p.Role.Contains(sortRole)).ToList();
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoleAndLocked(string userId, string selectedRole, bool locked)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Kiểm tra xem có vai trò nào không
            if (currentRoles.Any())
            {
                var currentRole = currentRoles.First();

                if (!string.Equals(selectedRole, currentRole, StringComparison.OrdinalIgnoreCase))
                {
                    // Chỉ thực hiện cập nhật vai trò nếu selectedRole khác currentRole
                    await _userManager.RemoveFromRoleAsync(user, currentRole);
                    await _userManager.AddToRoleAsync(user, selectedRole);

                    // Cập nhật dấu nhận bảo mật
                    await _userManager.UpdateSecurityStampAsync(user);
                }
            }


            if (locked)
            {
                var lockoutEndDate = DateTimeOffset.MaxValue;
                user.LockoutEnabled = true;
                user.LockoutEnd = lockoutEndDate;
            }
            else
            {
                user.LockoutEnabled = false;
                user.LockoutEnd = null;
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}