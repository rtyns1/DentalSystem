using DentalSystem.API.Data;
using DentalSystem.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using DentalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DentalSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PaymentsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/payments?search=...&page=1&pageSize=20
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _db.Payments.Include(p => p.Patient).Include(p => p.TreatmentPlan).AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p =>
                p.Patient.FullName.Contains(search) ||
                p.Reference.Contains(search));
        }

        var total = await query.CountAsync();

        var payments = await query
            .OrderByDescending(p => p.PaymentDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new { total, page, pageSize, payments });
    }

    // GET: api/payments/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var payment = await _db.Payments
            .Include(p => p.Patient)
            .Include(p => p.TreatmentPlan)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (payment == null)
            return NotFound();

        return Ok(payment);
    }

    // POST: api/payments
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Payment payment)
    {
        if (payment.Amount <= 0)
            return BadRequest("Amount must be greater than zero.");

        if (payment.PaymentDate == default)
            return BadRequest("Payment date is required.");

        // Check if patient exists
        var patientExists = await _db.Patients.AnyAsync(p => p.Id == payment.PatientId);
        if (!patientExists)
            return BadRequest("Patient does not exist.");

        // If TreatmentPlanId is provided, check it exists
        if (payment.TreatmentPlanId.HasValue)
        {
            var planExists = await _db.TreatmentPlans.AnyAsync(tp => tp.Id == payment.TreatmentPlanId.Value);
            if (!planExists)
                return BadRequest("Treatment plan does not exist.");
        }

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
    }

    // PUT: api/payments/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Payment updated)
    {
        if (id != updated.Id)
            return BadRequest("ID mismatch.");

        var existing = await _db.Payments.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Amount = updated.Amount;
        existing.PaymentDate = updated.PaymentDate;
        existing.PaymentMethod = updated.PaymentMethod;
        existing.Reference = updated.Reference;
        existing.Notes = updated.Notes;
        // Do not update PatientId or TreatmentPlanId – they should stay fixed

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/payments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var payment = await _db.Payments.FindAsync(id);
        if (payment == null)
            return NotFound();

        _db.Payments.Remove(payment);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}