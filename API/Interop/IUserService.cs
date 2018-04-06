using System;
using System.Threading.Tasks;
using MeetMusicModels.Models;

namespace API.Interop
{
    public interface IUserService
    {
        Task<User[]> GetUsers();
        Task<User> GetUser(Guid id);
        Task<UserMusicFamily[]> GetUserTopMusicFamilies(Guid id);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user, Guid id);
        Task ActivateUser(Guid id);
        Task DeleteUser(Guid id);
        Task<string> AuthenticateUser(string email, string password);
    }
}
