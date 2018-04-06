using System.Threading.Tasks;
using MeetMusicModels.InMemoryModels;
using MeetMusicModels.Models;

namespace API.Interop
{
    public interface ITastesManagementService
    {
        Task<MusicGenre[]> GetAllGenres();
        Task<MusicFamily[]> GetAllFamilies();
        Task UpdateFamilies(MusicFamilyUpdateModel[] model);
    }
}
