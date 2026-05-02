namespace PowerHouse.Models
{
    public class CheckIn
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public DateTime CheckInTime { get; set; } = DateTime.UtcNow;
    }
}
