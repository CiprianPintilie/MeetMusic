using System;
using System.Linq;
using System.Threading.Tasks;
using API.ExceptionMiddleware;
using API.Interop;
using Data.Context;
using MeetMusicModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Utils.Hash;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly MeetMusicDbContext _context;

        public UserService(MeetMusicDbContext context)
        {
            _context = context;
        }

        public async Task<User[]> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToArrayAsync();
                users = users.Where(u => !u.Deleted).ToArray();
                for (int i = 0; i < users.Length; i++)
                {
                    users[i].Password = null;
                }
                return users;
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task<User> GetUser(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id.ToString("D"));
                if (user == null || user.Deleted)
                    throw new HttpStatusCodeException(StatusCodes.Status404NotFound,
                        $"No user with the id '{id}' found");
                return user;
            }
            catch (HttpStatusCodeException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task<UserMusicFamily[]> GetUserTopMusicFamilies(Guid id)
        {
            try
            {
                var musicTastes = await _context.UserMusicFamilies.ToArrayAsync();
                return musicTastes.Where(t => new Guid(t.UserId).CompareTo(id) == 0).ToArray();
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                user.Id = Guid.NewGuid().ToString("D");
                user.Password = PasswordTool.HashPassword(user.Password);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                user.Password = null;
                return user;
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException != null && e.InnerException.Message.Contains("uplicate"))
                    throw new HttpStatusCodeException(StatusCodes.Status409Conflict, e.InnerException.Message);
                throw new HttpStatusCodeException(StatusCodes.Status400BadRequest, e.Message);
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task<User> UpdateUser(User user, Guid id)
        {
            try
            {
                var userModel = await _context.Users.FindAsync(id.ToString("D"));
                if (userModel == null || user.Deleted)
                    throw new HttpStatusCodeException(StatusCodes.Status404NotFound,
                        $"No user with the id '{id}' found");
                _context.Users.Update(CopyUser(user, userModel));
                await _context.SaveChangesAsync();
                return userModel;
            }
            catch (HttpStatusCodeException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task ActivateUser(Guid id)
        {
            try
            {
                var userModel = await _context.Users.FindAsync(id.ToString("D"));
                if (userModel == null)
                    throw new HttpStatusCodeException(StatusCodes.Status404NotFound,
                        $"No user with the id '{id}' found");
                userModel.Deleted = false;
                _context.Users.Update(userModel);
                await _context.SaveChangesAsync();
            }
            catch (HttpStatusCodeException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task DeleteUser(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id.ToString("D"));
                if (user == null || user.Deleted)
                    throw new HttpStatusCodeException(StatusCodes.Status404NotFound,
                        $"No user with the id '{id}' found");
                user.Deleted = true;
                user.DeletedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (HttpStatusCodeException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task<string> AuthenticateUser(string email, string password)
        {
            try
            {
                var users = await _context.Users.ToArrayAsync();
                var user = users.Single(u => u.Email == email);
                if (!PasswordTool.ValidatePassword(password, user.Password))
                    throw new HttpStatusCodeException(StatusCodes.Status401Unauthorized, "Invalid password");
                return user.Id;
            }
            catch (HttpStatusCodeException)
            {
                throw;
            }
            catch (ArgumentNullException)
            {
                throw new HttpStatusCodeException(StatusCodes.Status401Unauthorized, "User not found");
            }
            catch (InvalidOperationException)
            {
                throw new HttpStatusCodeException(StatusCodes.Status401Unauthorized, "User not found");
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        private User CopyUser(User sourceUserModel, User destUserModel)
        {
            destUserModel.FirstName = sourceUserModel.FirstName;
            destUserModel.LastName = sourceUserModel.LastName;
            destUserModel.Email = sourceUserModel.Email;
            destUserModel.Gender = sourceUserModel.Gender;
            destUserModel.AvatarUrl = sourceUserModel.AvatarUrl;
            destUserModel.PhoneNumber = sourceUserModel.PhoneNumber;
            destUserModel.BirthDate = sourceUserModel.BirthDate;
            destUserModel.Description = sourceUserModel.Description;
            destUserModel.UpdatedAt = DateTime.Now;

            return destUserModel;
        }
    }
}
