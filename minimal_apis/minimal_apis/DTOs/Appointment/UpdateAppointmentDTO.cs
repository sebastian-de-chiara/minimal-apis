using System;

namespace minimal_apis.DTOs.Appointment;

public class UpdateAppointmentDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int ServiceId { get; set; }
    public int LocationId { get; set; }
    public DateTime AppointmentTime { get; set; }
    public int DurationMinutes { get; set; }
    public string Status { get; set; } = null!;
}