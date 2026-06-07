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
    }
}