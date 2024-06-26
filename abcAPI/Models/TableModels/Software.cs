namespace abcAPI.Models.TableModels;

public class Software
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string CurrentVersion { get; set; }
    public string Category { get; set; } // e.g., Finance, Education, etc.
    public List<Contract> Contracts { get; set; }
    public List<Subscription> Subscriptions { get; set; }
}