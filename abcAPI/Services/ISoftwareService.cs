using abcAPI.Models.DTOs;

namespace abcAPI.Services;

public interface ISoftwareService
{
    Task AddSoftwareAsync(AddSoftwareDto software);
    Task<GetSoftwareDto> GetSoftwareAsync(int id);
    Task<List<GetSoftwareDto>> GetSoftwaresAsync();
}