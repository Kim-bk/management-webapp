﻿using System;
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
using Domain.AggregateModels.ProjectAggregate;
using Domain.AggregateModels.UserAggregate;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProjectRepository _projectRepository;

        public AccountService(IAccountRepository accountRepository, IUnitOfWork uniOfWork,
                    UserManager<ApplicationUser> userManager, IMapper mapper,
                    IRefreshTokenRepository refreshTokenRepository, IProjectRepository projectRepository) 
        {
            _refreshTokenRepository = refreshTokenRepository;
            _accountRepository = accountRepository;
            _projectRepository = projectRepository;
            _unitOfWork = uniOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserManagerResponse> Register(RegisterRequest model)
        {
            // 1. Validate input
            if (model == null)
            {
                throw new NullReferenceException("Register Model is null");
            }
            if(model.Password != model.ConfirmPassword)
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

        public async Task<ProjectManagerResponse> GetUserProjects(string userId)
        {
            try
            {
                // 1. Get all projects of user
                var userProjects = await _accountRepository.GetUserProjects(userId);

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
        }

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
    }
}