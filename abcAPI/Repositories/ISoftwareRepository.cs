using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;

namespace abcAPI.Repositories;

public interface ISoftwareRepository
{
    Task AddSoftwareAsync(AddSoftwareDto software);
    Task<GetSoftwareDto> GetSoftwareAsync(int id);
    Task<List<GetSoftwareDto>> GetSoftwaresAsync();

}