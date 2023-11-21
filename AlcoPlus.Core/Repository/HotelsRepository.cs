using AlcoPlus.Core.Contracts;
using AlcoPlus.Data;

namespace AlcoPlus.Core.Repository;

public class HotelsRepository : Repository<Hotel>, IHotelsRepository
{
    public HotelsRepository(AlcoPlusDbContext alcoPlusDbContext) : base(alcoPlusDbContext)
    {
    }
}
