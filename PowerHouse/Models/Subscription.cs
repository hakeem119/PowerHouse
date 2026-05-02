namespace PowerHouse.Models
{
    public enum SubscriptionStatus { Active, Expired }

    public class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public int PlanId { get; set; }
        public SubscriptionPlan? Plan { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public SubscriptionStatus Status =>
            EndDate >= DateTime.UtcNow ? SubscriptionStatus.Active : SubscriptionStatus.Expired;
    }
}
