using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerHouse.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace PowerHouse.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _db.Users
                .Include(u => u.MainBranch)
                .Include(u => u.Subscriptions).ThenInclude(s => s.Plan)
                .Include(u => u.CheckIns).ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return View(user);
        }
    }
}
