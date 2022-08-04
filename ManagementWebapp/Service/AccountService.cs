using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Domain.Accounts;
using Domain.DTOs.Responses;
using Domain.DTOS.Requests;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using Service.TokenGenratorServices;

namespace Service
{
    public class AccountService : IAccountService
    {
        private UserManager<ApplicationUser> _userManager;
        private IAccountRepository _accountRepository;
        private IUnitOfWork _unitOfWork;
        private AccessTokenGenerator _genAccessToken;
        private RefreshTokenGenerator _genRefreshToken;
        public AccountService(IAccountRepository accountRepository, IUnitOfWork uniOfWork, 
            AccessTokenGenerator genAccessToken, RefreshTokenGenerator genRefreshToken, UserManager<ApplicationUser> userManager)
        {
            _accountRepository = accountRepository;
            _unitOfWork = uniOfWork;
            _genAccessToken = genAccessToken;
            _genRefreshToken = genRefreshToken;
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

        public async Task<UserManagerResponse> UserLoginAsync(LoginRequest model)
        {
            // 1. Validate input
            if (model == null)
            {
                throw new NullReferenceException("Login Model is null");
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "User does not exist!",
                    IsSuccess = false,
                };
            }

            var rs = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!rs)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid password!",
                    IsSuccess = false,
                };
            }

            // 2. create access token for user
            var accessToken = _genAccessToken.Generate(user);

            // 3. create refresh token for user
           // var refreshToken = _genRefreshToken.Generate();
       
            return new UserManagerResponse
            {
                Message = new JwtSecurityTokenHandler().WriteToken(accessToken),
                IsSuccess = true,
                ExpireDate = accessToken.ValidTo
            };
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

        public Task<UserManagerResponse> UserLogout()
        {
            throw new NotImplementedException();
        }
    }
}
