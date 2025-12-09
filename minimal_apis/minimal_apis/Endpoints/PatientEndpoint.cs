using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using minimal_apis.DTOs.Patient;
using minimal_apis.Models;

namespace minimal_apis.Endpoints;

public static class PatientEndpoints
{
    public static void MapPatientEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/patients");

        // GET all patients
        group.MapGet("/", async (MyDbContext db) =>
        {
            var patients = await db.Patients
                .Select(p => new PatientDto
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    Email = p.Email
                })
                .ToListAsync();

            return patients;
        })
        .WithName("GetAllPatients");

        // GET patient by id
        group.MapGet("/{id}", async (int id, MyDbContext db) =>
        {
            var patient = await db.Patients
                .Where(p => p.PatientId == id)
                .Select(p => new PatientDto
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    Email = p.Email
                })
                .FirstOrDefaultAsync();

            return patient is not null ? Results.Ok(patient) : Results.NotFound();
        })
        .WithName("GetPatientById");

        // POST create patient
        group.MapPost("/", async (CreatePatientDto dto, MyDbContext db) =>
        {
            var patient = new Patient
            {
                Name = dto.Name,
                Email = dto.Email
            };

            db.Patients.Add(patient);
            await db.SaveChangesAsync();

            var createdPatient = new PatientDto
            {
                PatientId = patient.PatientId,
                Name = patient.Name,
                Email = patient.Email
            };

            return Results.Created($"/patients/{patient.PatientId}", createdPatient);
        })
        .WithName("CreatePatient");

        // PUT update patient
        group.MapPut("/{id}", async (int id, UpdatePatientDto dto, MyDbContext db) =>
        {
            var existing = await db.Patients.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.Name = dto.Name;
            existing.Email = dto.Email;

            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("UpdatePatient");

        // DELETE patient
        group.MapDelete("/{id}", async (int id, MyDbContext db) =>
        {
            var patient = await db.Patients.FindAsync(id);
            if (patient is null) return Results.NotFound();

            try
            {
                db.Patients.Remove(patient);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (DbUpdateException)
            {
                return Results.Conflict(new
                {
                    error = "Cannot delete patient",
                    detail = "This patient is referenced by existing appointments. Delete or reassign those appointments first."
                });
            }
        })
        .WithName("DeletePatient");
    }
}