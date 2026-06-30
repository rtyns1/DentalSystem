using DentalSystem.API.Data;
using DentalSystem.Models;
using DentalSystem.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TreatmentPlansController : ControllerBase
{
    private readonly AppDbContext _db;

    public TreatmentPlansController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/treatmentplans?search=...&page=1&pageSize=20
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _db.TreatmentPlans.Include(tp => tp.Patient).AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(tp =>
                tp.Patient.FullName.Contains(search) ||
                tp.Description.Contains(search));
        }

        var total = await query.CountAsync();

        var plans = await query
            .OrderByDescending(tp => tp.StartDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new { total, page, pageSize, plans });
    }

    // GET: api/treatmentplans/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var plan = await _db.TreatmentPlans
            .Include(tp => tp.Patient)
            .Include(tp => tp.Payments)
            .FirstOrDefaultAsync(tp => tp.Id == id);

        if (plan == null)
            return NotFound();

        return Ok(plan);
    }

    // POST: api/treatmentplans
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TreatmentPlan plan)
    {
        if (string.IsNullOrWhiteSpace(plan.Description))
            return BadRequest("Description is required.");

        if (plan.TotalCost <= 0)
            return BadRequest("Total cost must be greater than zero.");

        // Check if patient exists
        var patientExists = await _db.Patients.AnyAsync(p => p.Id == plan.PatientId);
        if (!patientExists)
            return BadRequest("Patient does not exist.");

        _db.TreatmentPlans.Add(plan);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = plan.Id }, plan);
    }

    // PUT: api/treatmentplans/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TreatmentPlan updated)
    {
        if (id != updated.Id)
            return BadRequest("ID mismatch.");

        var existing = await _db.TreatmentPlans.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Description = updated.Description;
        existing.TotalCost = updated.TotalCost;
        existing.StartDate = updated.StartDate;
        existing.ExpectedEndDate = updated.ExpectedEndDate;
        existing.Status = updated.Status;
        existing.Notes = updated.Notes;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/treatmentplans/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var plan = await _db.TreatmentPlans.FindAsync(id);
        if (plan == null)
            return NotFound();

        _db.TreatmentPlans.Remove(plan);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}