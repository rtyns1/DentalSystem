using DentalSystem.Shared.Models;
using DentalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DentalSystem.API.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<TreatmentPlan> TreatmentPlans { get; set; }
    public DbSet<Payment> Payments { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 3.1 – Patient → Visit (one‑to‑many, cascade delete)
        modelBuilder.Entity<Visit>()
            .HasOne(v => v.Patient)
            .WithMany(p => p.Visits)
            .HasForeignKey(v => v.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        // 3.2 – Patient → TreatmentPlan (one‑to‑many, cascade delete)
        modelBuilder.Entity<TreatmentPlan>()
            .HasOne(tp => tp.Patient)
            .WithMany(p => p.TreatmentPlans)
            .HasForeignKey(tp => tp.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        // 3.3 – Patient → Payment (one‑to‑many, cascade delete)
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Patient)
            .WithMany(p => p.Payments)
            .HasForeignKey(p => p.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        // 3.4 – TreatmentPlan → Payment (one‑to‑many, set null on delete)
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.TreatmentPlan)
            .WithMany(tp => tp.Payments)
            .HasForeignKey(p => p.TreatmentPlanId)
            .OnDelete(DeleteBehavior.SetNull);

        // 3.5 – Indexes for faster searching
        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.FullName);

        modelBuilder.Entity<Patient>()
            .HasIndex(p => p.CardNumber);
    }
}