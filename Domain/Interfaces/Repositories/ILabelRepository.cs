using System.Threading.Tasks;
using Domain.AggregateModels.TaskAggregate;

namespace Domain.Interfaces.Repositories
{
    public interface ILabelRepository
    {
        Task<Label> FindByIdAsync(int labelId);
        Task<Label> FindByNameAsync(string title);
    }
}
