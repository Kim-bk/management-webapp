using System.Threading.Tasks;
using Domain.DTOs.Requests;
using Domain.DTOs.Responses;

namespace Service.Interfaces
{
    public interface IProjectService
    {
        Task<UserManagerResponse> CreateUserProject(string userId, ProjectRequest model);
        Task<UserManagerResponse> AddMemberToProject(ProjectRequest model);
        Task<UserManagerResponse> CreateListTask(CommonRequest model);
        Task<ProjectManagerResponse> GetListTasks(int projectId);
    }
}
