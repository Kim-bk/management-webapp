using System.Threading.Tasks;
using Domain.Interfaces.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Domain.AggregateModels.TaskAggregate;

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

        public async Task<Label> FindByNameAsync(string title)
        {
            return await DbSet.Where(l => l.Title == title).FirstOrDefaultAsync();
        }
    }
}
