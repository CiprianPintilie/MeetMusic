using System;
using System.Threading.Tasks;
using MeetMusicModels.Models;

namespace API.Interop
{
    public interface IMusicFamilyService
    {
        Task<MusicFamily[]> GetMusicFamilies();
        Task<MusicFamily> GetMusicFamily(Guid id);
        Task<MusicGenre[]> GetFamilyMusicGenres(Guid id);
        Task<MusicFamily[]> CreateMusicFamily(MusicFamily[] musicFamilies);
        Task DeleteMusicFamily(Guid id);
    }
}
