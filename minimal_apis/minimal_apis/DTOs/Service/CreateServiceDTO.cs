namespace minimal_apis.DTOs.Service;

public class CreateServiceDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}