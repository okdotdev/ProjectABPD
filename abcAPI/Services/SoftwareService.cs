using abcAPI.Models.DTOs;
using abcAPI.Repositories;

namespace abcAPI.Services;

public class SoftwareService : ISoftwareService
{
    private readonly ISoftwareRepository _softwareRepository;

    public SoftwareService(ISoftwareRepository softwareRepository)
    {
        _softwareRepository = softwareRepository;
    }


    public async Task AddSoftwareAsync(AddSoftwareDto software)
    {
        await _softwareRepository.AddSoftwareAsync(software);
    }

    public async Task<GetSoftwareDto> GetSoftwareAsync(int id)
    {
        if(id <= 0)
        {
            throw new ArgumentException("Id must be greater than 0");
        }

        return await _softwareRepository.GetSoftwareAsync(id);
    }

    public async Task<List<GetSoftwareDto>> GetSoftwaresAsync()
    {
        return await _softwareRepository.GetSoftwaresAsync();
    }
}