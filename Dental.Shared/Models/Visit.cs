using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DentalSystem.Models;

namespace Dental.Shared.Models
{
    public class Visit
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Visit Date")]
        public DateTime VisitDate { get; set; }

        [Required]
        [Display(Name = "Procedure Done")]
        public string? ProcedureDone { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Next Appointment")]
        public DateTime? NextAppointment { get; set; }

        [Display(Name = "Appointment Status")]
        public string? AppointmentStatus { get; set; } = "Scheduled";

        [MaxLength (1000)]
        [Display(Name = "Doctor's Notes")]
        public string? DoctorNotes { get; set; }

        public virtual Patient? Patient { get; set; }
    }
}
