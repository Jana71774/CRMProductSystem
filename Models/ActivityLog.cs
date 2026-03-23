namespace CRMProductSystem.Models
{
    public class ActivityLog
    {
        public int ActivityId { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; } // From JOIN
        public int? UserId { get; set; }
        public string? Username { get; set; }     // From JOIN
        public string? Description { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}