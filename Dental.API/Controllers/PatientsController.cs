using DentalSystem.API.Data;
using DentalSystem.Shared.Models;
using DentalSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PatientsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/patients?search=...&page=1&pageSize=20
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _db.Patients.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p =>
                p.FullName.Contains(search) ||
                p.PhoneNumber.Contains(search) ||
                p.CardNumber.Contains(search));
        }

        var total = await query.CountAsync();

        var patients = await query
            .OrderBy(p => p.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new { total, page, pageSize, patients });
    }

    // GET: api/patients/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _db.Patients.FindAsync(id);
        if (patient == null)
            return NotFound();

        return Ok(patient);
    }

    // POST: api/patients
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Patient patient)
    {
        if (string.IsNullOrWhiteSpace(patient.FullName))
            return BadRequest("Full name is required.");

        // Duplicate check
        var duplicate = await _db.Patients.AnyAsync(p =>
            p.FullName == patient.FullName &&
            p.PhoneNumber == patient.PhoneNumber &&
            p.DateOfBirth == patient.DateOfBirth);

        if (duplicate)
            return Conflict("A patient with the same name, phone, and DOB already exists.");

        _db.Patients.Add(patient);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    // PUT: api/patients/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Patient updated)
    {
        if (id != updated.Id)
            return BadRequest("ID mismatch.");

        var existing = await _db.Patients.FindAsync(id);
        if (existing == null)
            return NotFound();

        // Update all editable fields
        existing.FullName = updated.FullName;
        existing.DateOfBirth = updated.DateOfBirth;
        existing.ManualAge = updated.ManualAge;
        existing.Gender = updated.Gender;
        existing.PhoneNumber = updated.PhoneNumber;
        existing.WhatsAppNumber = updated.WhatsAppNumber;
        existing.Address = updated.Address;
        existing.PostalAddress = updated.PostalAddress;
        existing.Residence = updated.Residence;
        existing.Occupation = updated.Occupation;
        existing.IsStudent = updated.IsStudent;
        existing.SchoolName = updated.SchoolName;
        existing.ClassGrade = updated.ClassGrade;
        existing.MedicalCondition1 = updated.MedicalCondition1;
        existing.MedicalCondition2 = updated.MedicalCondition2;
        existing.MedicalCondition3 = updated.MedicalCondition3;
        existing.Medication1 = updated.Medication1;
        existing.Medication2 = updated.Medication2;
        existing.Medication3 = updated.Medication3;
        existing.Allergy1 = updated.Allergy1;
        existing.Allergy2 = updated.Allergy2;
        existing.Allergy3 = updated.Allergy3;
        existing.EmergencyContactName = updated.EmergencyContactName;
        existing.EmergencyContactRelationship = updated.EmergencyContactRelationship;
        existing.EmergencyContactNumber = updated.EmergencyContactNumber;
        existing.ConsentForTreatment = updated.ConsentForTreatment;
        existing.CardNumber = updated.CardNumber;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/patients/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var patient = await _db.Patients.FindAsync(id);
        if (patient == null)
            return NotFound();

        _db.Patients.Remove(patient);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}