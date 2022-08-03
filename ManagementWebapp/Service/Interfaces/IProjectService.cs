using System.Threading.Tasks;
using Service.DTOs.Requests;
using Service.DTOS.Requests;
using Service.DTOS.Responses;

namespace Service.Interfaces
{
    public interface IProjectService
    {
        Task<UserManagerResponse> CreateUserProject(string userId, ProjectRequest model);
        Task<UserManagerResponse> AddMemberToProject(ProjectRequest model);
        Task<UserManagerResponse> CreateListTask(CommonRequest model);
    }
}
