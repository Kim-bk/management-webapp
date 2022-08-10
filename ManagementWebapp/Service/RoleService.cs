using System.Threading.Tasks;
using Domain.DTOs.Requests;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
                        IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IdentityResult> CreateRole(string nameRole)
        {
            // 1. Create role
            await _unitOfWork.BeginTransaction();
            var result = await _roleManager.CreateAsync(
                        new IdentityRole
                        {
                            Name = nameRole
                        });

            if (result.Succeeded)
            {
                await _unitOfWork.CommitTransaction();

                // 2. Return result
                return result;
            }
            await _unitOfWork.RollbackTransaction();
            return null;
        }

        public async Task<IdentityResult> AddRoleToUser(RoleRequest request)
        {
            // 1. Find user by id
            var user = await _userManager.FindByIdAsync(request.UserId);

            // 2. Add role to user
            var result = await _userManager.AddToRoleAsync(user, request.RoleName);

            // 3. Return result
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

                // 1. Find role
                var role = await _roleManager.FindByNameAsync(roleName);

                // 2. Find all users and remove from that role
                var users = await _userManager.Users.ToListAsync();
                foreach(var user in users)
                {
                    if (await _userManager.IsInRoleAsync(user, roleName))
                    {
                        await _userManager.RemoveFromRoleAsync(user, roleName);
                    }
                }

                // 3. Delete role
                var rs = await _roleManager.DeleteAsync(role);
                if (rs.Succeeded)
                {
                    await _unitOfWork.CommitTransaction();
                    return true;
                }

                await _unitOfWork.RollbackTransaction();
                return false;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }
    }
}
