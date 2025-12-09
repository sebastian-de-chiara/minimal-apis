using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using minimal_apis.DTOs.Appointment;
using minimal_apis.Models;

namespace minimal_apis.Endpoints;

public static class AppointmentEndpoints
{
    public static void MapAppointmentEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/appointments");

        // GET all appointments
        group.MapGet("/", async (MyDbContext db) =>
        {
            var appointments = await db.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Location)
                .Include(a => a.Service)
                .Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    ServiceId = a.ServiceId,
                    LocationId = a.LocationId,
                    AppointmentTime = a.AppointmentTime,
                    DurationMinutes = a.DurationMinutes,
                    Status = a.Status,
                    DoctorName = a.Doctor.Name,
                    DoctorSpecialty = a.Doctor.Specialty,
                    PatientName = a.Patient.Name,
                    LocationName = a.Location.Name,
                    ServiceName = a.Service.Name
                })
                .ToListAsync();

            return appointments;
        })
        .WithName("GetAllAppointments");

        // GET appointment by id
        group.MapGet("/{id}", async (int id, MyDbContext db) =>
        {
            var appointment = await db.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Location)
                .Include(a => a.Service)
                .Where(a => a.AppointmentId == id)
                .Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    ServiceId = a.ServiceId,
                    LocationId = a.LocationId,
                    AppointmentTime = a.AppointmentTime,
                    DurationMinutes = a.DurationMinutes,
                    Status = a.Status,
                    DoctorName = a.Doctor.Name,
                    DoctorSpecialty = a.Doctor.Specialty,
                    PatientName = a.Patient.Name,
                    LocationName = a.Location.Name,
                    ServiceName = a.Service.Name
                })
                .FirstOrDefaultAsync();

            return appointment is not null ? Results.Ok(appointment) : Results.NotFound();
        })
        .WithName("GetAppointmentById");

        // POST create appointment
        group.MapPost("/", async (CreateAppointmentDto dto, MyDbContext db) =>
        {
            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                ServiceId = dto.ServiceId,
                LocationId = dto.LocationId,
                AppointmentTime = dto.AppointmentTime,
                DurationMinutes = dto.DurationMinutes,
                Status = dto.Status
            };

            db.Appointments.Add(appointment);
            await db.SaveChangesAsync();

            // Load the full DTO
            var createdAppointment = await db.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Include(a => a.Location)
                .Include(a => a.Service)
                .Where(a => a.AppointmentId == appointment.AppointmentId)
                .Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    ServiceId = a.ServiceId,
                    LocationId = a.LocationId,
                    AppointmentTime = a.AppointmentTime,
                    DurationMinutes = a.DurationMinutes,
                    Status = a.Status,
                    DoctorName = a.Doctor.Name,
                    DoctorSpecialty = a.Doctor.Specialty,
                    PatientName = a.Patient.Name,
                    LocationName = a.Location.Name,
                    ServiceName = a.Service.Name
                })
                .FirstOrDefaultAsync();

            return Results.Created($"/appointments/{appointment.AppointmentId}", createdAppointment);
        })
        .WithName("CreateAppointment");

        // PUT update appointment
        group.MapPut("/{id}", async (int id, UpdateAppointmentDto dto, MyDbContext db) =>
        {
            var existing = await db.Appointments.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.PatientId = dto.PatientId;
            existing.DoctorId = dto.DoctorId;
            existing.ServiceId = dto.ServiceId;
            existing.LocationId = dto.LocationId;
            existing.AppointmentTime = dto.AppointmentTime;
            existing.DurationMinutes = dto.DurationMinutes;
            existing.Status = dto.Status;

            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("UpdateAppointment");

        // DELETE appointment
        group.MapDelete("/{id}", async (int id, MyDbContext db) =>
        {
            var appointment = await db.Appointments.FindAsync(id);
            if (appointment is null) return Results.NotFound();

            db.Appointments.Remove(appointment);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteAppointment");
    }
}