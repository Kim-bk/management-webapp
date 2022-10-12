using System;
using System.Threading.Tasks;
using Domain.Accounts;
using API.DTOs.Responses;
using API.DTOs.Requests;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using API.DTOs;
using Domain.AggregateModels.UserAggregate;
using System.Linq;
using API.IntegrationEvents;
using EventBus.Abstractions;

namespace User.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;


        public AccountService(IAccountRepository accountRepository, IUnitOfWork uniOfWork,
                    UserManager<ApplicationUser> userManager, IMapper mapper,
                    IRefreshTokenRepository refreshTokenRepository, IEventBus eventBus)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accountRepository = accountRepository;
            _unitOfWork = uniOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        public async Task<UserManagerResponse> Register(RegisterRequest model)
        {
            // 1. Validate input
            if (model == null)
            {
                throw new NullReferenceException("Register Model is null");
            }
            if (model.Password != model.ConfirmPassword)
            {
                throw new ArgumentException("Confirm password not match!");
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
                };
            }

            else
            {
                await _unitOfWork.RollbackTransaction();
                throw new ArgumentException("User created fail!");
            }
        }

        public async Task<ApplicationUser> Login(LoginRequest model)
        {
            // 1. Validate input
            if (model == null)
            {
                throw new ArgumentNullException("Empty input");
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

            throw new NullReferenceException("Cant login!");
        }

       /* public async Task<ProjectManagerResponse> GetUserProjects(string userId)
        {
            try
            {
                // 1. Get all projects of user
                var user = await _accountRepository.FindAsync(u => u.Id == userId);
                var userProjects = user.Projects.ToList();

                // 2. Map List Project to List Project DTO
                var mapProject = _mapper.Map<List<Project>, List<ProjectDTO>>(userProjects);

                // 3. Return message
                return new ProjectManagerResponse
                {
                    Message = "Get user projects success!",
                    Projects = mapProject,
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }*/

        public async Task<UserManagerResponse> Logout(string userId)
        {
            try
            {
                await _refreshTokenRepository.DeleteAll(userId);
                await _unitOfWork.CommitTransaction();
                return new UserManagerResponse
                {
                    Message = "User logout success!"
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<UserManagerResponse> Update(UserRequest request)
        {
            try
            {
                var findUser = await _userManager.FindByIdAsync(request.UserId);

                // 1. Update user email, phone number
                findUser.Update(request.Email, request.PhoneNumber);
                await _userManager.UpdateAsync(findUser);

                // 2. Send UserUpdatedIntegrationEvent
                var eventMessage = new UserUpdatedIntegrationEvent(request.UserId, request.Email, request.PhoneNumber);
                _eventBus.Publish(eventMessage);

                return new UserManagerResponse
                {
                    Message = "Update user success!",
                    //IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
