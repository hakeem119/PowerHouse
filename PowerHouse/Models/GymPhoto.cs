namespace PowerHouse.Models
{
    public class GymPhoto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string FileName { get; set; } = "";
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
    }
}
