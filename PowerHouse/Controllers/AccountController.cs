using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PowerHouse.Models;
using PowerHouse.VM;

namespace PowerHouse.Controllers
{
    public class AccountController : Controller
    {

        private readonly AppDbContext _db;
        public AccountController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Branches = await _db.Branches.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Branches = await _db.Branches.ToListAsync();
            if (!ModelState.IsValid) return View(model);

            if (await _db.Users.AnyAsync(u => u.Phone == model.Phone))
            {
                ModelState.AddModelError("Phone", "رقم الموبايل مسجل مسبقاً");
                return View(model);
            }

            var user = new User
            {
                Name = model.Name,
                Phone = model.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = UserRole.Member,
                MainBranchId = model.MainBranchId,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Phone == model.Phone);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "رقم الموبايل أو كلمة المرور غير صحيحة");
                return View(model);
            }

            await SignInUser(user);
            return user.Role == UserRole.Admin
                ? RedirectToAction("Index", "Admin")
                : RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("Phone", user.Phone)
        };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }
}
