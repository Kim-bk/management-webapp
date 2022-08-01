using System.Threading.Tasks;
using Service.DTOS.Requests;
using Service.DTOS.Responses;

namespace Service.Interfaces
{
    public interface IProjectService
    {
        Task<UserManagerResponse> CreateUserProject(string userId, ProjectRequest model);
        Task<UserManagerResponse> AddMemberToProject(string userId, int projectId);
        Task<UserManagerResponse> CreateListTask(string title, int projectId);
    }
}
