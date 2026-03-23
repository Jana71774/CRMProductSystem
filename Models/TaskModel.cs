using System.ComponentModel.DataAnnotations;

namespace CRMProductSystem.Models
{
    public class TaskModel
    {
        internal DateTime CreatedDate;

        public int TaskId { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; } // Pending / In Progress / Completed

        public DateTime? DueDate { get; set; }= DateTime.Now;

        // ================= RELATIONS =================

        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public int? ProductId { get; set; }
        public string? ProductName { get; set; }

        public int? AssignedTo { get; set; }
        public string? EmployeeName { get; set; }
    }
}