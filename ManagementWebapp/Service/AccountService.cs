using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using API.Entities;
using Domain;
using Domain.Accounts;
using Domain.Intefaces.Services;
using Domain.Shared;
using Service.TokenGenratorServices;

namespace Service
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;
        private IUnitOfWork _unitOfWork;
        private AccessTokenGenerator _genAccessToken;
        private RefreshTokenGenerator _genRefreshToken;
        public AccountService(IAccountRepository accountRepository, IUnitOfWork uniOfWork, 
                            AccessTokenGenerator genAccessToken, RefreshTokenGenerator genRefreshToken)
        {
            _accountRepository = accountRepository;
            _unitOfWork = uniOfWork;
            _genAccessToken = genAccessToken;
            _genRefreshToken = genRefreshToken;
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
                    Message = "Confirm password not macth!",
                    IsSuccess = false,
                };
            }
   
            // Begin a transaction
            await _unitOfWork.BeginTransaction();

            var rs = await _accountRepository.RegisterUser(model);

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
            
            var user = await _accountRepository.FindUserByName(model.UserName);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "User does not exist!",
                    IsSuccess = false,
                };
            }

            var rs = await _accountRepository.CheckPassword(user, model.Password);
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
            var refreshToken = _genRefreshToken.Generate();
       
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
                var rs = _accountRepository.GetUserProjects(userId);
                return rs;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<UserManagerResponse> CreateUserProject(ProjectRequest model, string userId)
        {
            await _unitOfWork.BeginTransaction();
            var rs = _accountRepository.CreateUserProject(model);
            if (rs != null)
            {
                if (_accountRepository.AssignUserToProject(rs, userId))
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

        public async Task<UserManagerResponse> UserLogout()
        {
            var rs = await _accountRepository.UserLogout();
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
            };
        }
    }
}
