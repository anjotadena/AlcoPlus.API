using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlcoPlus.Data.Configuration;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasData(
            new Hotel
            {
                Id = 1,
                Name = "Hotel 1",
                Address = "Hotel address 1",
                CountryId = 1,
                Rating = 4.4
            },
            new Hotel
            {
                Id = 2,
                Name = "Hotel 2",
                Address = "Hotel address 2",
                CountryId = 1,
                Rating = 3.5
            },
            new Hotel
            {
                Id = 3,
                Name = "Hotel 3",
                Address = "Hotel address 3",
                CountryId = 2,
                Rating = 4
            },
            new Hotel
            {
                Id = 4,
                Name = "Hotel 4",
                Address = "Hotel address 4",
                CountryId = 1,
                Rating = 4.5
            },
            new Hotel
            {
                Id = 5,
                Name = "Hotel 5",
                Address = "Hotel address 5",
                CountryId = 3,
                Rating = 2.5
            }
        );
    }
}
