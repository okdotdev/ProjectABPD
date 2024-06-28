using abcAPI.Models.DTOs;

namespace abcAPI.Services;

public interface IRaportService
{
    Task<byte[]> GeneratePdfReportAsync(List<GetContractDto> contracts);
}