using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Domain;
using Domain.Accounts;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Service.DTOS.Requests;
using Service.DTOS.Responses;
using Service.Interfaces;
using Service.TokenGenratorServices;

namespace Service
{
    public class AccountService : IAccountService
    {
        private UserManager<IdentityUser> _userManager;
        private IAccountRepository _accountRepository;
        private IUnitOfWork _unitOfWork;
        private AccessTokenGenerator _genAccessToken;
        private RefreshTokenGenerator _genRefreshToken;
        public AccountService(IAccountRepository accountRepository, IUnitOfWork uniOfWork, AccessTokenGenerator genAccessToken, 
                            RefreshTokenGenerator genRefreshToken, UserManager<IdentityUser> userManager)
        {
            _accountRepository = accountRepository;
            _unitOfWork = uniOfWork;
            _genAccessToken = genAccessToken;
            _genRefreshToken = genRefreshToken;
            _userManager = userManager;
        }
        public async Task<UserManagerResponse> RegisterUserAsync(RegisterRequest model)
        {
            // validate input
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
           
            // Begin a transaction
            await _unitOfWork.BeginTransaction();

            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.UserName,
            };

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
            // validate input
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

            // create access token for user
            var accessToken = _genAccessToken.Generate(user);

            // create refresh token for user
           // var refreshToken = _genRefreshToken.Generate();
       
            return new UserManagerResponse
            {
                Message = new JwtSecurityTokenHandler().WriteToken(accessToken),
                IsSuccess = true,
                ExpireDate = accessToken.ValidTo
            };
        }

        public async Task<IList<Project>> GetUserProjects(string userId)
        {
            try
            {
                var rs =  _accountRepository.GetUserProjects(userId);
                return rs;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<UserManagerResponse> CreateUserProject(ProjectRequest model, string userId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var rs = _accountRepository.CreateUserProject(model.Name);
                if (rs != null)
                {
                    if (_accountRepository.AddUserToProject(rs, userId))
                    {
                        await _unitOfWork.CommitTransaction();
                        return new UserManagerResponse
                        {
                            Message = "Create new project success!",
                            IsSuccess = true
                        };
                    }
                }

                await _unitOfWork.RollbackTransaction();
                return new UserManagerResponse
                {
                    IsSuccess = false,
                };
            }

            catch(Exception)
            {
                await _unitOfWork.RollbackTransaction();
                return new UserManagerResponse
                {
                    IsSuccess = false,
                };
            }
        }

        public async Task<UserManagerResponse> UserLogout()
        {
            return null;

           /* var rs = await _accountRepository.UserLogout();
            if (rs)
            {
                return new UserManagerResponse
                {
                    Message = "Log out success!",
                    IsSuccess = true,
                };
            }
            return new UserManagerResponse
            {
                Message = "Log out fail!",
                IsSuccess = false,
            };*/
        }
    }
}
