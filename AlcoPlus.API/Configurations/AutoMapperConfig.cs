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
        CreateMap<Country, GetCountryDetailsDto>().ReverseMap();
        CreateMap<Hotel, CreateHotelDto>().ReverseMap();
        CreateMap<Hotel, GetHotelDto>().ReverseMap();
    }
}
