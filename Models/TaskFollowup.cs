namespace CRMProductSystem.Models
{
    public class TaskFollowup
    {
        public int FollowupId { get; set; }

        public int TaskId { get; set; }

        public string? Notes { get; set; }

        public System.DateTime FollowupDate { get; set; }

        public string? SalesStatus { get; set; } 
        public int UserId { get; set; }
        public string? EmployeeName { get; set; }
    }
}