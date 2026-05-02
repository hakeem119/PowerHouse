namespace PowerHouse.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<User> Members { get; set; } = new List<User>();
        public ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    }
}
