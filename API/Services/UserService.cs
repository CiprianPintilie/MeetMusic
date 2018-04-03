using System;
using System.Threading.Tasks;
using API.Interop;
using Data.Context;
using MeetMusicModels.Models;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly MeetMusicDbContext _context;

        public UserService(MeetMusicDbContext context)
        {
            _context = context;
        }

        public Task<User[]> GetUsers()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserMusicFamily[]> GetUserTopMusicFamilies(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
