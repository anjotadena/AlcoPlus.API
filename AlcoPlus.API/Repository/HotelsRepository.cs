using AlcoPlus.API.Contracts;
using AlcoPlus.API.Data;
using AlcoPlus.API.Entities;
using AutoMapper;

namespace AlcoPlus.API.Repository;

public class HotelsRepository : Repository<Hotel>, IHotelsRepository
{
    public HotelsRepository(AlcoPlusDbContext alcoPlusDbContext, IMapper mapper) : base(alcoPlusDbContext, mapper)
    {
    }
}
