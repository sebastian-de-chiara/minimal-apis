using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using minimal_apis.DTOs.Location;
using minimal_apis.Models;

namespace minimal_apis.Endpoints;

public static class LocationEndpoints
{
    public static void MapLocationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/locations");

        // GET all locations
        group.MapGet("/", async (MyDbContext db) =>
        {
            var locations = await db.Locations
                .Select(l => new LocationDto
                {
                    LocationId = l.LocationId,
                    Name = l.Name,
                    City = l.City
                })
                .ToListAsync();

            return locations;
        })
        .WithName("GetAllLocations");

        // GET location by id
        group.MapGet("/{id}", async (int id, MyDbContext db) =>
        {
            var location = await db.Locations
                .Where(l => l.LocationId == id)
                .Select(l => new LocationDto
                {
                    LocationId = l.LocationId,
                    Name = l.Name,
                    City = l.City
                })
                .FirstOrDefaultAsync();

            return location is not null ? Results.Ok(location) : Results.NotFound();
        })
        .WithName("GetLocationById");

        // POST create location
        group.MapPost("/", async (CreateLocationDto dto, MyDbContext db) =>
        {
            var location = new Location
            {
                Name = dto.Name,
                City = dto.City
            };

            db.Locations.Add(location);
            await db.SaveChangesAsync();

            var createdLocation = new LocationDto
            {
                LocationId = location.LocationId,
                Name = location.Name,
                City = location.City
            };

            return Results.Created($"/locations/{location.LocationId}", createdLocation);
        })
        .WithName("CreateLocation");

        // PUT update location
        group.MapPut("/{id}", async (int id, UpdateLocationDto dto, MyDbContext db) =>
        {
            var existing = await db.Locations.FindAsync(id);
            if (existing is null) return Results.NotFound();

            existing.Name = dto.Name;
            existing.City = dto.City;

            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("UpdateLocation");

        // DELETE location
        group.MapDelete("/{id}", async (int id, MyDbContext db) =>
        {
            var location = await db.Locations.FindAsync(id);
            if (location is null) return Results.NotFound();

            try
            {
                db.Locations.Remove(location);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (DbUpdateException)
            {
                return Results.Conflict(new
                {
                    error = "Cannot delete location",
                    detail = "This location is referenced by existing appointments. Delete or reassign those appointments first."
                });
            }
        })
        .WithName("DeleteLocation");
    }
}