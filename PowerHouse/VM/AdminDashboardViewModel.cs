namespace PowerHouse.VM
{
    public class AdminDashboardViewModel
    {
        public int TotalMembers { get; set; }
        public int ActiveSubscriptions { get; set; }
        public int TotalVisitsToday { get; set; }
        public int TotalVisitsAllTime { get; set; }
        public List<BranchStatViewModel> BranchStats { get; set; } = new();
        public List<RecentCheckInViewModel> RecentCheckIns { get; set; } = new();
    }

    public class BranchStatViewModel
    {
        public string BranchName { get; set; } = "";
        public int VisitsToday { get; set; }
        public int TotalMembers { get; set; }
    }

    public class RecentCheckInViewModel
    {
        public string MemberName { get; set; } = "";
        public string BranchName { get; set; } = "";
        public DateTime CheckInTime { get; set; }
    }
}
