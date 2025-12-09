namespace minimal_apis.DTOs.Patient;

public class PatientDto
{
    public int PatientId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}