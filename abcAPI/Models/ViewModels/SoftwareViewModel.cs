using abcAPI.Models.DTOs;
using abcAPI.Models.TableModels;

namespace abcAPI.Models.ViewModels;

public class SoftwareViewModel
{
    public List<GetSoftwareDto> Softwares { get; set; }
}