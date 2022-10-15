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
using Domain.AggregateModels.ProjectAggregate;

namespace User.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;


        public AccountService(IAccountRepository accountRepository, IUnitOfWork uniOfWork,
                    UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _unitOfWork = uniOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }


        public async Task<ProjectManagerResponse> GetUserProjects(string userId)
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
