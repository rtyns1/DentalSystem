using DentalSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DentalSystem.Models
{
    public class TreatmentPlan
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public virtual Patient? Patient { get; set; }

        [Required]
        public string? Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime? ExpectedEndDate { get; set; }

        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? Notes { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}