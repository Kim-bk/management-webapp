using System.Threading.Tasks;
using API.DTOs.Requests;
using API.DTOs.Responses;

namespace Service.Interfaces
{
    public interface IProjectService
    {
        Task<UserManagerResponse> CreateUserProject(string userId, ProjectRequest model);
        Task<UserManagerResponse> AddMemberToProject(int projectId, ProjectRequest model);
        Task<UserManagerResponse> CreateListTask(int projectId, CommonRequest model);
        Task<ProjectManagerResponse> GetProject(int projectId);
        Task<UserManagerResponse> DeleteListTask(int projectId, int listTaskId);
        Task<ProjectManagerResponse> GetAllProjects();
        Task<ProjectManagerResponse> DeleteProject(int projectId);
        Task<ListTaskManagerResponse> GetListTask(int listTaskId);
    }
}
