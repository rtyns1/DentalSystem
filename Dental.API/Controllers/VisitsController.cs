using DentalSystem.API.Data;
using DentalSystem.Shared.Models;
using DentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitsController : ControllerBase
{
    private readonly AppDbContext _db;

    public VisitsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/visits?search=...&page=1&pageSize=20
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _db.Visits.Include(v => v.Patient).AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(v =>
                v.Patient.FullName.Contains(search) ||
                v.ProcedureDone.Contains(search));
        }

        var total = await query.CountAsync();

        var visits = await query
            .OrderByDescending(v => v.VisitDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new { total, page, pageSize, visits });
    }

    // GET: api/visits/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var visit = await _db.Visits
            .Include(v => v.Patient)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (visit == null)
            return NotFound();

        return Ok(visit);
    }

    // POST: api/visits
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Visit visit)
    {
        if (visit.VisitDate == default)
            return BadRequest("Visit date is required.");

        // Check if patient exists
        var patientExists = await _db.Patients.AnyAsync(p => p.Id == visit.PatientId);
        if (!patientExists)
            return BadRequest("Patient does not exist.");

        // Duplicate check: same patient, same date
        var duplicate = await _db.Visits.AnyAsync(v =>
            v.PatientId == visit.PatientId &&
            v.VisitDate.Date == visit.VisitDate.Date);

        if (duplicate)
            return Conflict("A visit already exists for this patient on this date.");

        _db.Visits.Add(visit);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = visit.Id }, visit);
    }

    // PUT: api/visits/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Visit updated)
    {
        if (id != updated.Id)
            return BadRequest("ID mismatch.");

        var existing = await _db.Visits.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.VisitDate = updated.VisitDate;
        existing.ProcedureDone = updated.ProcedureDone;
        existing.NextAppointment = updated.NextAppointment;
        existing.AppointmentStatus = updated.AppointmentStatus;
        existing.DoctorNotes = updated.DoctorNotes;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/visits/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var visit = await _db.Visits.FindAsync(id);
        if (visit == null)
            return NotFound();

        _db.Visits.Remove(visit);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}