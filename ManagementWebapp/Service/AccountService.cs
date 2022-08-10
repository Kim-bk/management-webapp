using System;
using System.Threading.Tasks;
using Domain.Accounts;
using Domain.DTOs.Responses;
using Domain.DTOS.Requests;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountRepository _accountRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(IAccountRepository accountRepository, IUnitOfWork uniOfWork, 
            UserManager<ApplicationUser> userManager, IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accountRepository = accountRepository;
            _unitOfWork = uniOfWork;
            _userManager = userManager;
        }
        public async Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model)
        {
            // 1. Validate input
            if (model == null)
            {
                throw new NullReferenceException("Register Model is null");
            }

            if(model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Confirm password not match!",
                    IsSuccess = false,
                };
            }
           
            // 2. Begin a transaction
            await _unitOfWork.BeginTransaction();

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName,
            };

            // 3. Assign GUID to Id user
            user.Id = Guid.NewGuid().ToString();
            var rs = await _userManager.CreateAsync(user, model.Password);

            if (rs.Succeeded)
            {
                await _unitOfWork.CommitTransaction();
                return new UserManagerResponse
                {
                    Message = "User created successfully!",
                    IsSuccess = true,
                };
            }

            else
            {
                await _unitOfWork.RollbackTransaction();
                return new UserManagerResponse
                {
                    Message = "User created failed!",
                    IsSuccess = false,
                };
            }
        }

        public async Task<ApplicationUser> UserLoginAsync(LoginRequest model)
        {
            // 1. Validate input
            if (model == null)
            {
                throw new NullReferenceException("Login Model is null");
            }
            
            // 2. Check username exists
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                // 3. Check password
                var password = await _userManager.CheckPasswordAsync(user, model.Password);
                if (password)
                {
                    return user;
                }
            }
            return null;
        }

        public ProjectManagerResponse GetUserProjects(string userId)
        {
            try
            {
                // 1. Get all projects of user
                var userProjects = _accountRepository.GetUserProjects(userId);

                // 3. Return message
                return new ProjectManagerResponse
                {
                    Message = "Get user projects success!",
                    Projects = userProjects,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new ProjectManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = false
                };
            }
        }

        public async Task<UserManagerResponse> Logout(string userId)
        {
            await _unitOfWork.BeginTransaction();

            try
            {
                await _refreshTokenRepository.DeleteAll(userId);
                await _unitOfWork.CommitTransaction();
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "User logout success!"
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                return new UserManagerResponse
                {
                    Message = e.ToString(),
                    IsSuccess = false,
                };
            }
        }
    }
}
