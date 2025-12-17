using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using minimal_apis.DTOs;
using minimal_apis.Models;

namespace minimal_apis.Endpoints;

public static class DoctorEndpoints
{
    public static void MapDoctorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/doctors");

        // GET all doctors
        group.MapGet("/", async (MyDbContext db, IMemoryCache cache) =>
        {
            const string cacheKey = "doctors_list";

            // Try and get from cache first
            if (cache.TryGetValue(cacheKey, out List<DoctorDto>? cachedDoctors))
            {
                return cachedDoctors;
            }

            // Not in cache so get from the database instead
            var doctors = await db.Doctors
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    Name = d.Name,
                    Specialty = d.Specialty
                })
                .ToListAsync();

            // Store in cache
            cache.Set(cacheKey, doctors, TimeSpan.FromHours(24));

            return doctors;
        })
        .WithName("GetAllDoctors");

        // GET doctor by id
        group.MapGet("/{id}", async (int id, MyDbContext db, IMemoryCache cache) =>
        {
            var cacheKey = $"doctor_{id}";

            // Try and get from cache first
            if (cache.TryGetValue(cacheKey, out DoctorDto? cachedDoctor))
            {
                return Results.Ok(cachedDoctor);
            }

            // Not in cache, get from database
            var doctor = await db.Doctors
                .Where(d => d.DoctorId == id)
                .Select(d => new DoctorDto
                {
                    DoctorId = d.DoctorId,
                    Name = d.Name,
                    Specialty = d.Specialty
                })
                .FirstOrDefaultAsync();

            if (doctor is null)
            {
                return Results.NotFound();
            }

            // Store in cache
            cache.Set(cacheKey, doctor, TimeSpan.FromHours(24));

            return Results.Ok(doctor);
        })
        .WithName("GetDoctorById");

        // POST create doctor
        group.MapPost("/", async (CreateDoctorDto dto, MyDbContext db, IMemoryCache cache) =>
        {
            var doctor = new Doctor
            {
                Name = dto.Name,
                Specialty = dto.Specialty
            };

            db.Doctors.Add(doctor);
            await db.SaveChangesAsync();

            // Invalidate cache, it's now incomplete
            cache.Remove("doctors_list");

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
        group.MapPut("/{id}", async (int id, UpdateDoctorDto dto, MyDbContext db, IMemoryCache cache) =>
        {
            var existing = await db.Doctors.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.Name = dto.Name;
            existing.Specialty = dto.Specialty;

            await db.SaveChangesAsync();

            // Invalidate both cache keys. List has stale data, individual doc is outdated
            cache.Remove("doctors_list");
            cache.Remove($"doctor_{id}");

            return Results.NoContent();
        })
        .WithName("UpdateDoctor");

        // DELETE doctor
        group.MapDelete("/{id}", async (int id, MyDbContext db, IMemoryCache cache) =>
        {
            var doctor = await db.Doctors.FindAsync(id);
            if (doctor is null) return Results.NotFound();

            try
            {
                db.Doctors.Remove(doctor);
                await db.SaveChangesAsync();

                // Invalidate both cache keys. List includes deleted doctor, individual doc shouldn't be cached
                cache.Remove("doctors_list");
                cache.Remove($"doctor_{id}");

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