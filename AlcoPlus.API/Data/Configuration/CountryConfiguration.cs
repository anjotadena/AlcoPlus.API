using AlcoPlus.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlcoPlus.API.Data.Configuration;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(
            new Country
            {
                Id = 1,
                Name = "Philippines",
                ShortName = "PH",
            },
            new Country
            {
                Id = 2,
                Name = "Japan",
                ShortName = "JP",
            },
            new Country
            {
                Id = 3,
                Name = "Bahamas",
                ShortName = "BH",
            }
        );
    }
}
