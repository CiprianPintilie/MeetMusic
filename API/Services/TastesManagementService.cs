using System;
using System.Linq;
using System.Threading.Tasks;
using API.ExceptionMiddleware;
using API.Interop;
using Data.Context;
using MeetMusicModels.InMemoryModels;
using MeetMusicModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class TastesManagementService : ITastesManagementService
    {
        private readonly MeetMusicDbContext _context;

        public TastesManagementService(MeetMusicDbContext context)
        {
            _context = context;
        }

        public async Task<MusicGenre[]> GetAllGenres()
        {
            try
            {
                var musicGenres = await _context.MusicGenres.ToArrayAsync();
                musicGenres = musicGenres.Where(g => !g.Deleted).ToArray();
                return musicGenres;
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task<MusicFamily[]> GetAllFamilies()
        {
            try
            {
                var musicFamilies = await _context.MusicFamilies.ToArrayAsync();
                musicFamilies = musicFamilies.Where(f => !f.Deleted).ToArray();
                return musicFamilies;
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task UpdateFamilies(MusicFamilyUpdateModel[] model)
        {
            try
            {
                //Updates music families
                var familyNames = model.Select(i => i.FamilyName).ToHashSet();
                var existingFamilyNames = await _context.MusicFamilies.Select(f => f.Name).ToArrayAsync();
                foreach (var family in familyNames)
                {
                    if (!existingFamilyNames.Contains(family))
                        await _context.MusicFamilies.AddAsync(new MusicFamily
                        {
                            Id = Guid.NewGuid().ToString("D"),
                            Name = family
                        });
                }

                await _context.SaveChangesAsync();

                var existingFamilies = await _context.MusicFamilies.ToArrayAsync();
                //Updates music genres and associate their families
                var genres = model.Select(i => i).ToHashSet();
                var existingGenres = await _context.MusicGenres.ToArrayAsync();
                foreach (var genre in genres)
                {
                    //If genre allready exist update family id
                    if (existingGenres.Any(g => g.Name.Equals(genre.FamilyName)))
                    {
                        existingGenres.Single(g => g.Name.Equals(genre.FamilyName)).FamilyId =
                            existingFamilies.Single(f => f.Name.Equals(genre.FamilyName)).Id;
                    }
                    else
                    {
                        await _context.MusicGenres.AddAsync(new MusicGenre
                        {
                            Id = Guid.NewGuid().ToString("D"),
                            Name = genre.GenreName,
                            FamilyId = existingFamilies.Single(f => f.Name.Equals(genre.FamilyName)).Id
                        });
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
