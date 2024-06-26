namespace abcAPI.Models.DTOs;

public class AddSoftwareDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CurrentVersion { get; set; }
    public string Category { get; set; }
}