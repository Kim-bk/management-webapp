using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs.Requests;
using Microsoft.AspNetCore.Identity;

namespace Service.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRole(string nameRole);
        Task<IdentityResult> AddRoleToUser(RoleRequest request);
        Task<List<IdentityRole>> GetAllRoles();
        Task<bool> CheckUserRole(string userId, string roleName);
        Task<bool> DeleteRole(string userId, string roleName);
    }
}
