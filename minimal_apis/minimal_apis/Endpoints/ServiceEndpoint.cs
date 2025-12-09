using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using minimal_apis.DTOs.Service;
using minimal_apis.Models;

namespace minimal_apis.Endpoints;

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/services");

        // GET all services
        group.MapGet("/", async (MyDbContext db) =>
        {
            var services = await db.Services
                .Select(s => new ServiceDto
                {
                    ServiceId = s.ServiceId,
                    Name = s.Name,
                    Price = s.Price
                })
                .ToListAsync();

            return services;
        })
        .WithName("GetAllServices");

        // GET service by id
        group.MapGet("/{id}", async (int id, MyDbContext db) =>
        {
            var service = await db.Services
                .Where(s => s.ServiceId == id)
                .Select(s => new ServiceDto
                {
                    ServiceId = s.ServiceId,
                    Name = s.Name,
                    Price = s.Price
                })
                .FirstOrDefaultAsync();

            return service is not null ? Results.Ok(service) : Results.NotFound();
        })
        .WithName("GetServiceById");

        // POST create service
        group.MapPost("/", async (CreateServiceDto dto, MyDbContext db) =>
        {
            var service = new Service
            {
                Name = dto.Name,
                Price = dto.Price
            };

            db.Services.Add(service);
            await db.SaveChangesAsync();

            var createdService = new ServiceDto
            {
                ServiceId = service.ServiceId,
                Name = service.Name,
                Price = service.Price
            };

            return Results.Created($"/services/{service.ServiceId}", createdService);
        })
        .WithName("CreateService");

        // PUT update service
        group.MapPut("/{id}", async (int id, UpdateServiceDto dto, MyDbContext db) =>
        {
            var existing = await db.Services.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.Name = dto.Name;
            existing.Price = dto.Price;

            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("UpdateService");

        // DELETE service
        group.MapDelete("/{id}", async (int id, MyDbContext db) =>
        {
            var service = await db.Services.FindAsync(id);
            if (service is null) return Results.NotFound();

            try
            {
                db.Services.Remove(service);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (DbUpdateException)
            {
                return Results.Conflict(new
                {
                    error = "Cannot delete service",
                    detail = "This service is referenced by existing appointments. Delete or reassign those appointments first."
                });
            }
        })
        .WithName("DeleteService");
    }
}