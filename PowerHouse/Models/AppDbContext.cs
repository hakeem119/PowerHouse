using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PowerHouse.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<CheckIn> CheckIns => Set<CheckIn>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {

            // Seed branches
            mb.Entity<Branch>().HasData(
                new Branch { Id = 1, Name = "الفرع الرئيسي - المعادي", Address = "شارع النصر، المعادي، القاهرة", CreatedAt = DateTime.UtcNow },
                new Branch { Id = 2, Name = "فرع مدينة نصر", Address = "شارع عباس العقاد، مدينة نصر، القاهرة", CreatedAt = DateTime.UtcNow },
                new Branch { Id = 3, Name = "فرع الزمالك", Address = "شارع 26 يوليو، الزمالك، القاهرة", CreatedAt = DateTime.UtcNow }
            );

            // Seed plans
            mb.Entity<SubscriptionPlan>().HasData(
                new SubscriptionPlan { Id = 1, Name = "شهري", DurationInDays = 30, Price = 350, Description = "اشتراك شهر واحد - أنسب للمبتدئين" },
                new SubscriptionPlan { Id = 2, Name = "ثلاثة أشهر", DurationInDays = 90, Price = 900, Description = "اشتراك 3 أشهر - وفر 14%" },
                new SubscriptionPlan { Id = 3, Name = "سنوي", DurationInDays = 365, Price = 3000, Description = "اشتراك سنوي - أفضل قيمة، وفر 28%" }
            );

            mb.Entity<User>().HasData(new User
            {
                Id = 1,
                Name = "Admin",
                Phone = "01000000000",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = UserRole.Admin,
                MainBranchId = 1,
                CreatedAt = DateTime.UtcNow
            });
        }

    }
}
