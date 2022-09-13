﻿using System.Threading.Tasks;
using API.DTOs.Responses;
using API.DTOs.Requests;
using Domain.AggregateModels.UserAggregate;

namespace Service.Interfaces
{
    public interface IAccountService
    {
        Task<UserManagerResponse> Register(RegisterRequest model);
        Task<ApplicationUser> Login(LoginRequest model);
        Task<ProjectManagerResponse> GetUserProjects(string userId);
        Task<UserManagerResponse> Logout(string userId);
    }
}