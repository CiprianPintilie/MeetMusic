using System;
using System.Threading.Tasks;
using MeetMusicModels.InMemoryModels;
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
        Task UpdateUserTastes(Guid id, UserMusicFamily[] models);
        Task ActivateUser(Guid id);
        Task<MatchModel[]> MatchUser(Guid id, MatchParametersModel model);
        Task DeleteUser(Guid id);
        Task<string> AuthenticateUser(string email, string password);
    }
}
