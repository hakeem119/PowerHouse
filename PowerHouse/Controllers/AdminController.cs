using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PowerHouse.Models;
using PowerHouse.VM;

namespace PowerHouse.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {

        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var today = DateTime.UtcNow.Date;
            var vm = new AdminDashboardViewModel
            {
                TotalMembers = await _db.Users.CountAsync(u => u.Role == UserRole.Member),
                ActiveSubscriptions = await _db.Subscriptions.CountAsync(s => s.EndDate >= DateTime.UtcNow),
                TotalVisitsToday = await _db.CheckIns.CountAsync(c => c.CheckInTime.Date == today),
                TotalVisitsAllTime = await _db.CheckIns.CountAsync(),
                BranchStats = await _db.Branches.Select(b => new BranchStatViewModel
                {
                    BranchName = b.Name,
                    VisitsToday = b.CheckIns.Count(c => c.CheckInTime.Date == today),
                    TotalMembers = b.Members.Count
                }).ToListAsync(),
                RecentCheckIns = await _db.CheckIns
                    .Include(c => c.User)
                    .Include(c => c.Branch)
                    .OrderByDescending(c => c.CheckInTime)
                    .Take(10)
                    .Select(c => new RecentCheckInViewModel
                    {
                        MemberName = c.User!.Name,
                        BranchName = c.Branch!.Name,
                        CheckInTime = c.CheckInTime
                    }).ToListAsync()
            };
            return View(vm);
        }

        public async Task<IActionResult> Members()
        {
            var members = await _db.Users
                .Include(u => u.MainBranch)
                .Include(u => u.Subscriptions).ThenInclude(s => s.Plan)
                .Where(u => u.Role == UserRole.Member)
                .ToListAsync();
            return View(members);
        }

        public async Task<IActionResult> Branches()
        {
            var branches = await _db.Branches
                .Include(b => b.Members)
                .Include(b => b.CheckIns)
                .ToListAsync();
            return View(branches);
        }

        [HttpGet]
        public IActionResult AddBranch() => View();

        [HttpPost]
        public async Task<IActionResult> AddBranch(Branch branch)
        {
            if (!ModelState.IsValid) return View(branch);
            branch.CreatedAt = DateTime.UtcNow;
            _db.Branches.Add(branch);
            await _db.SaveChangesAsync();
            TempData["Success"] = "تم إضافة الفرع بنجاح!";
            return RedirectToAction("Branches");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _db.Branches.FindAsync(id);
            if (branch != null) { _db.Branches.Remove(branch); await _db.SaveChangesAsync(); }
            return RedirectToAction("Branches");
        }

        // ===== GYM PHOTOS =====
        public async Task<IActionResult> Photos()
        {
            var photos = await _db.GymPhotos
                .Include(p => p.Branch)
                .OrderByDescending(p => p.UploadedAt)
                .ToListAsync();
            ViewBag.Branches = await _db.Branches.ToListAsync();
            return View(photos);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile photoFile, string title, string description, int? branchId)
        {
            if (photoFile == null || photoFile.Length == 0)
            {
                TempData["Error"] = "يرجى اختيار صورة.";
                return RedirectToAction("Photos");
            }

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var ext = Path.GetExtension(photoFile.FileName).ToLower();
            if (!allowed.Contains(ext))
            {
                TempData["Error"] = "نوع الملف غير مدعوم. استخدم JPG أو PNG أو WEBP.";
                return RedirectToAction("Photos");
            }

            if (photoFile.Length > 5 * 1024 * 1024)
            {
                TempData["Error"] = "حجم الصورة يجب أن يكون أقل من 5 ميجابايت.";
                return RedirectToAction("Photos");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "gym");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsFolder, uniqueName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await photoFile.CopyToAsync(stream);

            var photo = new GymPhoto
            {
                Title = title ?? "صورة الجيم",
                Description = description ?? "",
                FileName = uniqueName,
                BranchId = branchId == 0 ? null : branchId,
                UploadedAt = DateTime.UtcNow
            };

            _db.GymPhotos.Add(photo);
            await _db.SaveChangesAsync();

            TempData["Success"] = "تم رفع الصورة بنجاح!";
            return RedirectToAction("Photos");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var photo = await _db.GymPhotos.FindAsync(id);
            if (photo != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "gym", photo.FileName);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

                _db.GymPhotos.Remove(photo);
                await _db.SaveChangesAsync();
                TempData["Success"] = "تم حذف الصورة.";
            }
            return RedirectToAction("Photos");
        }

        public async Task<IActionResult> CheckIns()
        {
            var checkIns = await _db.CheckIns
                .Include(c => c.User)
                .Include(c => c.Branch)
                .OrderByDescending(c => c.CheckInTime)
                .Take(100)
                .ToListAsync();
            return View(checkIns);
        }
    }
}
