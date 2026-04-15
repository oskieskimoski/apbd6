using System.ComponentModel.DataAnnotations;

namespace apbd6.DTOs;

public class CreateRoomDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string BuildingCode { get; set; } =  string.Empty;
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public bool HasProjector { get; set; }
    public bool IsActive { get; set; }
}