namespace PowerHouse.Models
{
    public enum UserRole { Member, Admin }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.Member;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? MainBranchId { get; set; }
        public Branch? MainBranch { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();
    }
}
