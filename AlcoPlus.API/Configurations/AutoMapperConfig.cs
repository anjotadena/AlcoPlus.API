using AlcoPlus.API.Entities;
using AlcoPlus.API.Models.Countries;
using AlcoPlus.API.Models.Hotels;
using AutoMapper;

namespace AlcoPlus.API.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Country, CreateCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();

        // Hotel entities
        CreateMap<Hotel, CreateHotelDto>().ReverseMap();
        CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
        CreateMap<Hotel, HotelDto>().ReverseMap();
    }
}
