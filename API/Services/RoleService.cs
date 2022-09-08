using System.Threading.Tasks;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using API.DTOs.Requests;
using API.Services;
using API.Services.Interfaces;
using Domain.AggregateModels.UserAggregate;
using System;

namespace Service
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
       
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
                        IUnitOfWork unitOfWork, IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateRole(string nameRole)
        {
            try
            {
                // 1. Valid input
                if (String.IsNullOrEmpty(nameRole) || String.IsNullOrWhiteSpace(nameRole))
                {
                    throw new ArgumentNullException("Empty input!");
                }

                // 2. Create role
                var result = await _roleManager.CreateAsync(
                                new IdentityRole
                                {
                                    Name = nameRole
                                });
                await _unitOfWork.CommitTransaction();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IdentityResult> AddRoleToUser(RoleRequest request)
        {
            // 1. Find user by id
            var user = await _userManager.FindByIdAsync(request.UserId);

            // 2. Add role to user
            await _unitOfWork.BeginTransaction();
            
            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (result.Succeeded)
            {
                await _unitOfWork.CommitTransaction();
                return result;
            }
            await _unitOfWork.RollbackTransaction();
            return result;
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            return await _roleManager.Roles.OrderBy(n => n.Name).ToListAsync();
        }

        public async Task<bool> CheckUserRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<bool> DeleteRole(string userId, string roleName)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                // 1. Get all users in role
                var users = await _userManager.GetUsersInRoleAsync(roleName);

                // 2. Remove role from user
                foreach (var user in users)
                {
                   await _userManager.RemoveFromRoleAsync(user, roleName);
                }

                // 3. Find role
                var role = await _roleManager.FindByNameAsync(roleName);

                // 4. Delete role
                var rs = await _roleManager.DeleteAsync(role);
               
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                throw e;
            }
        }
    }
}
