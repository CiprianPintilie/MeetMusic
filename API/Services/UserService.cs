using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ExceptionMiddleware;
using API.Interop;
using Data.Context;
using MeetMusicModels.InMemoryModels;
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

        public async Task UpdateUserTastes(Guid userId, UserMusicFamily[] models)
        {
            try
            {
                var musicTastes = await _context.UserMusicFamilies.ToArrayAsync();
                var userMusicTastes = musicTastes.Where(t => new Guid(t.UserId).CompareTo(userId) == 0).ToArray();
                if (userMusicTastes.Any())
                {
                    _context.UserMusicFamilies.RemoveRange(userMusicTastes);
                    await _context.SaveChangesAsync();
                }
                foreach (var model in models)
                {
                    model.UserId = userId.ToString("D");
                    await _context.UserMusicFamilies.AddAsync(model);
                }
                await _context.SaveChangesAsync();
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

        public async Task<MatchModel[]> MatchUser(Guid id, MatchParametersModel model)
        {
            try
            {
                var matchedUsers = new List<MatchModel>();
                var userTastes = await GetUserTopMusicFamilies(id);
                var topUserTastes = userTastes.OrderBy(t => t.Rank).Take(3).ToArray();
                var users = await GetUsers();
                foreach (var item in users)
                {
                    if (item.Deleted || new Guid(item.Id).CompareTo(id) == 0)
                        continue;
                    if (model != null)
                        if (model.Gender != 0 && model.Gender != item.Gender)
                            continue;

                    var matchScore = 0.0;
                    var tastes = await GetUserTopMusicFamilies(new Guid(item.Id));
                    var topTastes = tastes.OrderBy(t => t.Rank).Take(3).ToArray();
                    if (!topUserTastes.Select(t => t.FamilyId).Any(x => topTastes.Select(t => t.FamilyId).Any(y => y == x)))
                        continue;
                    foreach (var taste in topUserTastes)
                    {
                        var matchedTaste = topTastes.SingleOrDefault(t => t.FamilyId.Equals(taste.FamilyId));
                        if (matchedTaste == null)
                            continue;
                        matchScore += ComputeMatchScore(matchedTaste.Rank, taste.Rank);
                    }
                    if (matchScore > 0)
                        matchedUsers.Add(new MatchModel
                        {
                            User = item,
                            MatchScore = matchScore
                        });
                }
                return matchedUsers.OrderByDescending(m => m.MatchScore).ToArray();
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

        private double ComputeMatchScore(int matchedTastePosition, int userTastePosition)
        {
            int scoreValue;
            switch (userTastePosition)
            {
                case 1:
                    scoreValue = 60;
                    break;
                case 2:
                    scoreValue = 40;
                    break;
                case 3:
                    scoreValue = 30;
                    break;
                default:
                    throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, "Something went wrong during tastes ranking");
            }
            return Math.Round((double)scoreValue / matchedTastePosition, 1);
        }
    }
}
