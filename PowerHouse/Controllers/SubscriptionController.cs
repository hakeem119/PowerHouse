using Microsoft.AspNetCore.Mvc;
using PowerHouse.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace PowerHouse.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly AppDbContext _db;
        public SubscriptionController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var plans = await _db.SubscriptionPlans.ToListAsync();
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var active = await _db.Subscriptions
                .Include(s => s.Plan)
                .Where(s => s.UserId == userId && s.EndDate >= DateTime.UtcNow)
                .FirstOrDefaultAsync();
            ViewBag.ActiveSubscription = active;
            return View(plans);
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(int planId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var plan = await _db.SubscriptionPlans.FindAsync(planId);
            if (plan == null) return NotFound();

            // Expire existing
            var existing = await _db.Subscriptions
                .Where(s => s.UserId == userId && s.EndDate >= DateTime.UtcNow)
                .ToListAsync();
            // Just add new on top
            var start = DateTime.UtcNow;
            var sub = new Subscription
            {
                UserId = userId,
                PlanId = planId,
                StartDate = start,
                EndDate = start.AddDays(plan.DurationInDays)
            };
            _db.Subscriptions.Add(sub);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"تم الاشتراك في خطة {plan.Name} بنجاح!";
            return RedirectToAction("Index");
        }
    }
}
