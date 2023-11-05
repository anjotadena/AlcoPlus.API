using AlcoPlus.API.Models.Countries;
using System.ComponentModel.DataAnnotations;

namespace AlcoPlus.API.Models.Hotels;

public class CreateHotelDto
{
    [Required]
    public string Name { get; set; }

    public string Address { get; set; }

    public double Rating { get; set; }

    [Required]
    public int CountryId { get; set; }
}
