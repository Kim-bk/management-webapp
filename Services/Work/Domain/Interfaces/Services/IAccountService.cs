using System.Threading.Tasks;
using API.DTOs.Responses;
using API.DTOs.Requests;
using Domain.AggregateModels.UserAggregate;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<ProjectManagerResponse> GetUserProjects(string userId);
    }
}
