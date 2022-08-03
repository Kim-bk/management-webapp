using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class LabelRepostiory : Repository<Label>, ILabelRepository
    {
        public LabelRepostiory(DbFactory dbFactory) : base(dbFactory)
        { }

        public async Task<Label> FindByIdAsync(int labelId)
        {
            return await DbSet.FindAsync(labelId);
        }
    }
}
