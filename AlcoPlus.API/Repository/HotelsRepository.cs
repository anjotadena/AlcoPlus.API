using AlcoPlus.API.Contracts;
using AlcoPlus.API.Data;
using AlcoPlus.API.Entities;

namespace AlcoPlus.API.Repository;

public class HotelsRepository : Repository<Hotel>, IHotelsRepository
{
    public HotelsRepository(AlcoPlusDbContext alcoPlusDbContext) : base(alcoPlusDbContext)
    {
    }
}
