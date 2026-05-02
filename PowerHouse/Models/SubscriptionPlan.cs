namespace PowerHouse.Models
{
    public class SubscriptionPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int DurationInDays { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = "";
    }
}
