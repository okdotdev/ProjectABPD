using abcAPI.Exceptions;
using abcAPI.Models;
using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;

namespace abcAPI.Repositories;

public class SoftwareRepository : ISoftwareRepository
{
    private readonly AppDbContext _context;

    public SoftwareRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddSoftwareAsync(AddSoftwareDto software)
    {
        Software newSoftware = new()
        {
            Name = software.Name,
            Description = software.Description,
            CurrentVersion = software.CurrentVersion,
            Category = software.Category
        };

        _context.Softwares.Add(newSoftware);
        await _context.SaveChangesAsync();
    }

    public async Task<GetSoftwareDto> GetSoftwareAsync(int id)
    {
        Software? software = await _context.Softwares.FindAsync(id);

        if (software == null)
        {
            throw new SoftwareNotFoundException("Software not found");
        }

        GetSoftwareDto getSoftwareDto = new()
        {
            Id = software.Id,
            Name = software.Name,
            Description = software.Description,
            CurrentVersion = software.CurrentVersion,
            Category = software.Category
        };
        return getSoftwareDto;
    }

    public async Task<List<GetSoftwareDto>> GetSoftwaresAsync()
    {
        List<Software> softwares = await _context.Softwares.ToListAsync();

        return softwares.Select(software => new GetSoftwareDto()
            {
                Id = software.Id,
                Name = software.Name,
                Description = software.Description,
                CurrentVersion = software.CurrentVersion,
                Category = software.Category
            })
            .ToList();
    }
}