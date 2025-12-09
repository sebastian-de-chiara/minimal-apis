using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using minimal_apis.Endpoints;
using minimal_apis.Models;

namespace minimal_apis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<MyDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.")));

            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapAppointmentEndpoints();
            app.MapDoctorEndpoints();
            app.MapPatientEndpoints();
            app.MapLocationEndpoints(); 
            app.MapServiceEndpoints();

            app.Run();
        }
    }
}
