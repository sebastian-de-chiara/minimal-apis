using System;

namespace minimal_apis.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public int ServiceId { get; set; }

    public int LocationId { get; set; }

    public DateTime AppointmentTime { get; set; }

    public int DurationMinutes { get; set; }

    public string Status { get; set; } = null!;

    public virtual Doctor Doctor { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
