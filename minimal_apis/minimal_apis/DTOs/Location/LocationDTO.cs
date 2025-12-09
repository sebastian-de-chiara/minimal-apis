namespace minimal_apis.DTOs.Location;

public class LocationDto
{
    public int LocationId { get; set; }
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
}