﻿using UserHub.Api.Domain;

namespace UserHub.Api.Data.Users
{
    public class UserRepository : IUserRepository
    {
        private static List<User> _users = new();

        public Task<bool> CreateUser(User user)
        {
            if (user != null)
            {
                user.Id = Guid.NewGuid();
                _users.Add(user);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public bool DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.FromResult(_users);
        }

        public async Task<User> GetUser(Guid id)
        {
            return await Task.FromResult(_users.FirstOrDefault(user => user.Id == id)!);
        }
    }
}
