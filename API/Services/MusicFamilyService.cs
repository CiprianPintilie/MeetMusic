using System;
using System.Threading.Tasks;
using API.Interop;
using Data.Context;
using MeetMusicModels.Models;

namespace API.Services
{
    public class MusicFamilyService : IMusicFamilyService
    {
        private readonly MeetMusicDbContext _context;

        public MusicFamilyService(MeetMusicDbContext context)
        {
            _context = context;
        }

        public Task<MusicFamily[]> GetMusicFamilies()
        {
            throw new NotImplementedException();
        }

        public Task<MusicFamily> GetMusicFamily(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MusicGenre[]> GetFamilyMusicGenres(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MusicFamily[]> CreateMusicFamily(MusicFamily[] musicFamilies)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMusicFamily(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
