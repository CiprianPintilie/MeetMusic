using System;
using System.Threading.Tasks;
using API.Interop;
using Data.Context;
using MeetMusicModels.Models;

namespace API.Services
{
    public class MusicGenreService : IMusicGenreService
    {
        private readonly MeetMusicDbContext _context;

        public MusicGenreService(MeetMusicDbContext context)
        {
            _context = context;
        }

        public Task<MusicGenre[]> GetMusicGenres()
        {
            throw new NotImplementedException();
        }

        public Task<MusicGenre> GetMusicGenre(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MusicFamily> GetGenreFamily(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MusicGenre[]> CreateMusicGenres(MusicGenre[] musicGenres)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMusicGenre(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
