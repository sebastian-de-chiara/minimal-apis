using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using minimal_apis.DTOs;
using minimal_apis.Models;

namespace minimal_apis.Endpoints;

public static class DoctorEndpoints
{
    public static void MapDoctorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/doctors");

        // GET all doctors
        group.MapGet("/", async (MyDbContext db) =>
        {
            var doctors = await db.Doctors
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    Name = d.Name,
                    Specialty = d.Specialty
                })
                .ToListAsync();

            return doctors;
        })
        .WithName("GetAllDoctors");

        // GET doctor by id
        group.MapGet("/{id}", async (int id, MyDbContext db) =>
        {
            var doctor = await db.Doctors
                .Where(d => d.DoctorId == id)
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    Name = d.Name,
                    Specialty = d.Specialty
                })
                .FirstOrDefaultAsync();

            return doctor is not null ? Results.Ok(doctor) : Results.NotFound();
        })
        .WithName("GetDoctorById");

        // POST create doctor
        group.MapPost("/", async (CreateDoctorDto dto, MyDbContext db) =>
        {
            var doctor = new Doctor
            {
                Name = dto.Name,
                Specialty = dto.Specialty
            };

            db.Doctors.Add(doctor);
            await db.SaveChangesAsync();

            // Return the created doctor
            var createdDoctor = new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                Name = doctor.Name,
                Specialty = doctor.Specialty
            };

            return Results.Created($"/doctors/{doctor.DoctorId}", createdDoctor);
        })
        .WithName("CreateDoctor");

        // PUT update doctor
        group.MapPut("/{id}", async (int id, UpdateDoctorDto dto, MyDbContext db) =>
        {
            var existing = await db.Doctors.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.Name = dto.Name;
            existing.Specialty = dto.Specialty;

            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("UpdateDoctor");

        // DELETE doctor
        group.MapDelete("/{id}", async (int id, MyDbContext db) =>
        {
            var doctor = await db.Doctors.FindAsync(id);
            if (doctor is null) return Results.NotFound();

            try
            {
                db.Doctors.Remove(doctor);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (DbUpdateException)
            {
                return Results.Conflict(new
                {
                    error = "Cannot delete doctor",
                    detail = "This doctor is referenced by existing appointments. Delete or reassign those appointments first."
                });
            }
        })
        .WithName("DeleteDoctor");
    }
}