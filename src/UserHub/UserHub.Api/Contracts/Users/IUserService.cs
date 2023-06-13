﻿using UserHub.Api.Domain;
using UserHub.Api.Services.Users;

namespace UserHub.Api.Contracts.Users
{
    public interface IUserService
    {
        User CreateUser(User newUser);
        List<User> GetAllUsers();
        User GetUserById(Guid id);
    }
}
