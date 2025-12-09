namespace minimal_apis.DTOs;

public class DoctorDto
{
    public int DoctorId { get; set; }
    public string Name { get; set; } = null!;
    public string Specialty { get; set; } = null!;
}