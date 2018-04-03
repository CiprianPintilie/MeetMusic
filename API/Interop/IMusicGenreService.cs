using System;
using System.Threading.Tasks;
using MeetMusicModels.Models;

namespace API.Interop
{
    public interface IMusicGenreService
    {
        Task<MusicGenre[]> GetMusicGenres();
        Task<MusicGenre> GetMusicGenre(Guid id);
        Task<MusicFamily> GetGenreFamily(Guid id);
        Task<MusicGenre[]> CreateMusicGenres(MusicGenre[] musicGenres);
        Task DeleteMusicGenre(Guid id);
    }
}
