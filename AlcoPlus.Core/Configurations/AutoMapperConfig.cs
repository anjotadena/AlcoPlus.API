using AlcoPlus.Data;
using AlcoPlus.Core.Models.Countries;
using AlcoPlus.Core.Models.Hotels;
using AlcoPlus.Core.Models.Users;
using AutoMapper;

namespace AlcoPlus.Core.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // Country entities
        CreateMap<Country, CreateCountryDto>().ReverseMap();
        CreateMap<Country, GetCountryDto>().ReverseMap();
        CreateMap<Country, CountryDto>().ReverseMap();

        // Hotel entities
        CreateMap<Hotel, CreateHotelDto>().ReverseMap();
        CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
        CreateMap<Hotel, HotelDto>().ReverseMap();

        // User entities
        CreateMap<ApiUserDto, ApiUser>().ReverseMap();
    }
}
