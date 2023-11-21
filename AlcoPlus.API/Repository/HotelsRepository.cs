using AlcoPlus.API.Contracts;
using AlcoPlus.Data;

namespace AlcoPlus.API.Repository;

public class HotelsRepository : Repository<Hotel>, IHotelsRepository
{
    public HotelsRepository(AlcoPlusDbContext alcoPlusDbContext) : base(alcoPlusDbContext)
    {
    }
}
