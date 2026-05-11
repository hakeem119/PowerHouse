using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using PowerHouse.Models;


namespace PowerHouse.Controllers
{
    [Authorize(Roles = "Member")]

    public class CheckInController : Controller
    {
        private readonly AppDbContext _db;
        public CheckInController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var branches = await _db.Branches.ToListAsync();
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var history = await _db.CheckIns
                .Include(c => c.Branch)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CheckInTime)
                .Take(20)
                .ToListAsync();
            ViewBag.Branches = branches;
            ViewBag.History = history;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckIn(int branchId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Check active subscription
            var hasActive = await _db.Subscriptions
                .AnyAsync(s => s.UserId == userId && s.EndDate >= DateTime.UtcNow);
            if (!hasActive)
            {
                TempData["Error"] = "ليس لديك اشتراك نشط. يرجى تجديد اشتراكك أولاً.";
                return RedirectToAction("Index");
            }

            var checkIn = new CheckIn
            {
                UserId = userId,
                BranchId = branchId,
                CheckInTime = DateTime.UtcNow
            };
            _db.CheckIns.Add(checkIn);
            await _db.SaveChangesAsync();

            var branch = await _db.Branches.FindAsync(branchId);
            TempData["Success"] = $"تم تسجيل دخولك في {branch?.Name} بنجاح! 💪";
            return RedirectToAction("Index");
        }
    }
}
