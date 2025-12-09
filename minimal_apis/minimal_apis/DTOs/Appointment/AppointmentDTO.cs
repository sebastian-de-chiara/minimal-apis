using System;

namespace minimal_apis.DTOs.Appointment;

public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int ServiceId { get; set; }
    public int LocationId { get; set; }
    public DateTime AppointmentTime { get; set; }
    public int DurationMinutes { get; set; }
    public string Status { get; set; } = null!;
    public string DoctorName { get; set; } = null!;
    public string DoctorSpecialty { get; set; } = null!;
    public string PatientName { get; set; } = null!;
    public string LocationName { get; set; } = null!;
    public string ServiceName { get; set; } = null!;
}